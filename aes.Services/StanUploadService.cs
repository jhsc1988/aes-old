using aes.Models;
using aes.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using Serilog;
using System.Globalization;
using System.Text;
using aes.Repositories.UnitOfWork;

namespace aes.Services
{
    public class StanUploadService : IStanUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public StanUploadService(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork, ILogger logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<JsonResult> Upload(HttpRequest Request, string userName)
        {

            string _loggerTemplate = System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType.FullName + ", " + "User: " + userName + ", " + "msg: ";

            {
                StanUpdate StanUpdate = _unitOfWork.StanUpdate.GetLatest();
                if (StanUpdate is not null && StanUpdate.UpdateComplete == false && StanUpdate.Interrupted == false)
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

            string folderPath = _webHostEnvironment.WebRootPath + @"\Uploaded";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            foreach (IFormFile file in files)
            {

                FileStream fs;
                TextFieldParser reader;
                string fileName = _webHostEnvironment.WebRootPath + $@"\Uploaded\{file.FileName}";
                string ff = file.ContentType;

                //if (!file.ContentType.Equals("application/vnd.ms-excel"))                
                if (!file.ContentType.Equals("text/csv"))
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

                        StanUpdate lastSuccessful = _unitOfWork.StanUpdate.GetLatestSuccessfulUpdate();
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

                        StanUpdate StanUpdate = new()
                        {
                            UpdateBegan = DateTime.Now,
                            ExecutedBy = userName,
                        };

                        _unitOfWork.StanUpdate.Add(StanUpdate);
                        _ = await _unitOfWork.Complete();

                        while (!reader.EndOfData)
                        {
                            try
                            {
                                string[] StanDetailsCsvLine = reader.ReadFields();
                                int startIndex = StanDetailsCsvLine.Length == 16 ? 1 : 2;
                                Stan stan = await _unitOfWork.Stan.FindExact(e => e.StanId == int.Parse(StanDetailsCsvLine[startIndex + 0]));

                                if (stan != null)
                                {
                                    stan.SifraObjekta = int.Parse(StanDetailsCsvLine[startIndex + 1]);
                                    stan.Vrsta = StanDetailsCsvLine[startIndex + 2].Equals("") ? null : StanDetailsCsvLine[startIndex + 2];
                                    stan.Adresa = StanDetailsCsvLine[startIndex + 3].Equals("") ? null : StanDetailsCsvLine[startIndex + 3];
                                    stan.Kat = StanDetailsCsvLine[startIndex + 4].Equals("") ? null : StanDetailsCsvLine[startIndex + 4];
                                    stan.BrojSTana = StanDetailsCsvLine[startIndex + 5].Equals("") ? null : StanDetailsCsvLine[startIndex + 5];
                                    stan.Naselje = StanDetailsCsvLine[startIndex + 6].Equals("") ? null : StanDetailsCsvLine[startIndex + 6];
                                    stan.Četvrt = StanDetailsCsvLine[startIndex + 7].Equals("") ? null : StanDetailsCsvLine[startIndex + 7];
                                    stan.Površina = double.TryParse(StanDetailsCsvLine[startIndex + 8], style, culture, out double _d) ? _d : null;
                                    stan.StatusKorištenja = StanDetailsCsvLine[startIndex + 9].Equals("") ? null : StanDetailsCsvLine[startIndex + 9];
                                    stan.Korisnik = StanDetailsCsvLine[startIndex + 10].Equals("") ? null : StanDetailsCsvLine[startIndex + 10];
                                    stan.Vlasništvo = StanDetailsCsvLine[startIndex + 11].Equals("") ? null : StanDetailsCsvLine[startIndex + 11];
                                    stan.DioNekretnine = StanDetailsCsvLine[startIndex + 12].Equals("") ? null : StanDetailsCsvLine[startIndex + 12];
                                    stan.Sektor = StanDetailsCsvLine[startIndex + 13].Equals("") ? null : StanDetailsCsvLine[startIndex + 13];
                                    stan.Status = StanDetailsCsvLine[startIndex + 14].Equals("") ? null : StanDetailsCsvLine[startIndex + 14];
                                    _ = await _unitOfWork.Stan.Update(stan);
                                }

                                else
                                {
                                    Stan newStan = new()
                                    {
                                        StanId = int.Parse(StanDetailsCsvLine[startIndex + 0]),
                                        SifraObjekta = int.Parse(StanDetailsCsvLine[startIndex + 1]),
                                        Vrsta = StanDetailsCsvLine[startIndex + 2].Equals("") ? null : StanDetailsCsvLine[startIndex + 2],
                                        Adresa = StanDetailsCsvLine[startIndex + 3].Equals("") ? null : StanDetailsCsvLine[startIndex + 3],
                                        Kat = StanDetailsCsvLine[startIndex + 4].Equals("") ? null : StanDetailsCsvLine[startIndex + 4],
                                        BrojSTana = StanDetailsCsvLine[startIndex + 5].Equals("") ? null : StanDetailsCsvLine[startIndex + 5],
                                        Naselje = StanDetailsCsvLine[startIndex + 6].Equals("") ? null : StanDetailsCsvLine[startIndex + 6],
                                        Četvrt = StanDetailsCsvLine[startIndex + 7].Equals("") ? null : StanDetailsCsvLine[startIndex + 7],
                                        Površina = double.TryParse(StanDetailsCsvLine[startIndex + 8], style, culture, out double _d) ? _d : null,
                                        StatusKorištenja = StanDetailsCsvLine[startIndex + 9].Equals("") ? null : StanDetailsCsvLine[startIndex + 9],
                                        Korisnik = StanDetailsCsvLine[startIndex + 10].Equals("") ? null : StanDetailsCsvLine[startIndex + 10],
                                        Vlasništvo = StanDetailsCsvLine[startIndex + 11].Equals("") ? null : StanDetailsCsvLine[startIndex + 11],
                                        DioNekretnine = StanDetailsCsvLine[startIndex + 12].Equals("") ? null : StanDetailsCsvLine[startIndex + 12],
                                        Sektor = StanDetailsCsvLine[startIndex + 13].Equals("") ? null : StanDetailsCsvLine[startIndex + 13],
                                        Status = StanDetailsCsvLine[startIndex + 14].Equals("") ? null : StanDetailsCsvLine[startIndex + 14],
                                    };

                                    _unitOfWork.Stan.Add(newStan);
                                    _ = await _unitOfWork.Complete();

                                    //_logger.Information(_loggerTemplate + "Added: " + newStan.StanId);
                                }

                            }
                            catch (Exception e)
                            {
                                _logger.Error(_loggerTemplate + e.Message);

                                StanUpdate.Interrupted = true;
                                _ = await _unitOfWork.StanUpdate.Update(StanUpdate);
                            }
                        }

                        StanUpdate.UpdateEnded = DateTime.Now;
                        StanUpdate.UpdateComplete = true;
                        StanUpdate.DateOfData = (DateTime)dateTimeOfData;
                        _ = await _unitOfWork.StanUpdate.Update(StanUpdate);

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