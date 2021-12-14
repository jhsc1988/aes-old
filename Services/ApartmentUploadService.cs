using aes.Models;
using aes.Repository.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using aes.Services.IServices;
using Serilog;

namespace aes.Services
{
    public class ApartmentUploadService : IApartmentUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public ApartmentUploadService(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork, ILogger logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<JsonResult> Upload(HttpRequest Request, string userName)
        {

            string _loggerTemplate = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + ", " + "User: " + userName + ", " + "msg: ";

            {
                ApartmentUpdate apartmentUpdate = _unitOfWork.ApartmentUpdate.getLatest();
                if (apartmentUpdate is not null && apartmentUpdate.UpdateComplete == false && apartmentUpdate.Interrupted == false)
                {

                    _logger.Information(_loggerTemplate + "there is already active update");

                    return new JsonResult(new
                    {
                        success = false,
                        message = "Ažuriranje je već u tijeku"
                    });
                }
            }

            IFormFileCollection files;
            try
            {
                files = Request.Form.Files;
            }
            catch (Exception e)
            {

                _logger.Error(_loggerTemplate + e.Message);

                return new JsonResult(new
                {
                    success = false,
                    message = e.Message
                });
            }

            foreach (IFormFile file in files)
            {

                FileStream fs;
                TextFieldParser reader;
                string fileName = _webHostEnvironment.WebRootPath + $@"\Uploaded\{file.FileName}";

                if (!file.ContentType.Equals("application/vnd.ms-excel"))
                {
                    string message = "not .csv file";
                    _logger.Information(_loggerTemplate + message);

                    return new JsonResult(new
                    {
                        success = false,
                        message
                    });
                }

                if (file.Length > 20480000)
                {
                    string message = "Too big, max 2 mb";
                    _logger.Information(_loggerTemplate + message);

                    return new JsonResult(new
                    {
                        success = false,
                        message
                    });
                }

                try
                {
                    NumberStyles style = NumberStyles.Number;
                    DateTimeStyles dateStyle = DateTimeStyles.None;
                    CultureInfo culture = CultureInfo.CreateSpecificCulture("hr-HR");

                    using (fs = File.Create(fileName))
                    {
                        file.CopyTo(fs);
                    }

                    using (reader = new TextFieldParser(new StreamReader(fileName, Encoding.GetEncoding(culture.TextInfo.ANSICodePage), true)))
                    {
                        reader.HasFieldsEnclosedInQuotes = true;
                        reader.SetDelimiters(";");
                        string[] txt = null;

                        for (int i = 0; i < 12; i++)
                        {
                            txt = reader.ReadFields(); // skip 12 lines
                        }

                        ApartmentUpdate lastSuccessful = _unitOfWork.ApartmentUpdate.getLatestSuccessfulUpdate();
                        DateTime? dateTimeOfData = DateTime.TryParse(txt[4], culture, dateStyle, out DateTime date) ? date : null;
                        DateTime now = DateTime.Now;

                        if (dateTimeOfData is null)
                        {
                            string message = "Date of data is empty";

                            _logger.Information(_loggerTemplate + message);

                            return new JsonResult(new
                            {
                                success = false,
                                message
                            });
                        }

                        if (lastSuccessful is not null && dateTimeOfData <= lastSuccessful.DateOfData)
                        {
                            string message = "Data cannot be older";

                            _logger.Information(_loggerTemplate + message);

                            return new JsonResult(new
                            {
                                success = false,
                                message
                            });
                        }

                        //if (lastSuccessful.UpdateEnded > now.AddHours(-24) && lastSuccessful.UpdateEnded <= now)
                        //{
                        //    string message = "Update possible every 24h";

                        //    _logger.Information(_loggerTemplate + message);

                        //    return new JsonResult(new
                        //    {
                        //        success = false,
                        //        message
                        //    });
                        //}


                        for (int i = 0; i < 23; i++)
                        {
                            _ = reader.ReadLine(); // skip 23 lines
                        }

                        ApartmentUpdate apartmentUpdate = new()
                        {
                            UpdateBegan = DateTime.Now,
                            ExecutedBy = userName,
                        };

                        _unitOfWork.ApartmentUpdate.Add(apartmentUpdate);
                        _ = await _unitOfWork.Complete();

                        while (!reader.EndOfData)
                        {
                            try
                            {
                                string[] apartmentDetailsCsvLine = reader.ReadFields();
                                int startIndex = apartmentDetailsCsvLine.Length == 16 ? 1 : 2;
                                Stan stan = await _unitOfWork.Apartment.FindExact(e => e.StanId == int.Parse(apartmentDetailsCsvLine[startIndex + 0]));

                                if (stan != null)
                                {
                                    stan.SifraObjekta = int.Parse(apartmentDetailsCsvLine[startIndex + 1]);
                                    stan.Vrsta = apartmentDetailsCsvLine[startIndex + 2].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 2];
                                    stan.Adresa = apartmentDetailsCsvLine[startIndex + 3].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 3];
                                    stan.Kat = apartmentDetailsCsvLine[startIndex + 4].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 4];
                                    stan.BrojSTana = apartmentDetailsCsvLine[startIndex + 5].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 5];
                                    stan.Naselje = apartmentDetailsCsvLine[startIndex + 6].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 6];
                                    stan.Četvrt = apartmentDetailsCsvLine[startIndex + 7].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 7];
                                    stan.Površina = double.TryParse(apartmentDetailsCsvLine[startIndex + 8], style, culture, out double _d) ? _d : null;
                                    stan.StatusKorištenja = apartmentDetailsCsvLine[startIndex + 9].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 9];
                                    stan.Korisnik = apartmentDetailsCsvLine[startIndex + 10].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 10];
                                    stan.Vlasništvo = apartmentDetailsCsvLine[startIndex + 11].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 11];
                                    stan.DioNekretnine = apartmentDetailsCsvLine[startIndex + 12].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 12];
                                    stan.Sektor = apartmentDetailsCsvLine[startIndex + 13].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 13];
                                    stan.Status = apartmentDetailsCsvLine[startIndex + 14].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 14];
                                    _ = await _unitOfWork.Apartment.Update(stan);
                                }

                                else
                                {
                                    Stan newApartment = new()
                                    {
                                        StanId = int.Parse(apartmentDetailsCsvLine[startIndex + 0]),
                                        SifraObjekta = int.Parse(apartmentDetailsCsvLine[startIndex + 1]),
                                        Vrsta = apartmentDetailsCsvLine[startIndex + 2].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 2],
                                        Adresa = apartmentDetailsCsvLine[startIndex + 3].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 3],
                                        Kat = apartmentDetailsCsvLine[startIndex + 4].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 4],
                                        BrojSTana = apartmentDetailsCsvLine[startIndex + 5].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 5],
                                        Naselje = apartmentDetailsCsvLine[startIndex + 6].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 6],
                                        Četvrt = apartmentDetailsCsvLine[startIndex + 7].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 7],
                                        Površina = double.TryParse(apartmentDetailsCsvLine[startIndex + 8], style, culture, out double _d) ? _d : null,
                                        StatusKorištenja = apartmentDetailsCsvLine[startIndex + 9].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 9],
                                        Korisnik = apartmentDetailsCsvLine[startIndex + 10].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 10],
                                        Vlasništvo = apartmentDetailsCsvLine[startIndex + 11].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 11],
                                        DioNekretnine = apartmentDetailsCsvLine[startIndex + 12].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 12],
                                        Sektor = apartmentDetailsCsvLine[startIndex + 13].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 13],
                                        Status = apartmentDetailsCsvLine[startIndex + 14].Equals("") ? null : apartmentDetailsCsvLine[startIndex + 14],
                                    };

                                    _unitOfWork.Apartment.Add(newApartment);
                                    _ = await _unitOfWork.Complete();

                                    //_logger.Information(_loggerTemplate + "Added: " + newApartment.StanId);
                                }

                            }
                            catch (Exception e)
                            {
                                _logger.Error(_loggerTemplate + e.Message);

                                apartmentUpdate.Interrupted = true;
                                _ = await _unitOfWork.ApartmentUpdate.Update(apartmentUpdate);
                            }
                        }

                        apartmentUpdate.UpdateEnded = DateTime.Now;
                        apartmentUpdate.UpdateComplete = true;
                        apartmentUpdate.DateOfData = (DateTime)dateTimeOfData;
                        _ = await _unitOfWork.ApartmentUpdate.Update(apartmentUpdate);

                    }

                }

                catch (Exception e)
                {
                    _logger.Error(_loggerTemplate + e.Message);

                    return new JsonResult(new
                    {
                        success = false,
                        message = e.Message
                    });
                }

                finally
                {
                    File.Delete(fileName);
                }
            }

            {
                string message = "Uspješno uploadano";
                _logger.Information(_loggerTemplate + message);

                return new JsonResult(new
                {
                    success = true,
                    message
                });
            }
        }
    }
}
