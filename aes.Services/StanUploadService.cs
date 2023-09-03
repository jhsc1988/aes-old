using aes.Models;
using aes.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using Serilog;
using System.Globalization;
using System.Text;
using aes.UnitOfWork;

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
        public async Task<JsonResult> Upload(HttpRequest request, string userName)
        {

            string loggerTemplate = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.FullName + ", " + "User: " + userName + ", " + "msg: ";

            {
                StanUpdate stanUpdate = _unitOfWork.StanUpdate.GetLatestAsync();
                if (stanUpdate is not null && stanUpdate.UpdateComplete == false && stanUpdate.Interrupted == false)
                {

                    _logger.Information(loggerTemplate + "there is already active update");

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
                files = request.Form.Files;
            }
            catch (Exception e)
            {

                _logger.Error(loggerTemplate + e.Message);

                return new JsonResult(new
                {
                    success = false,
                    message = e.Message
                });
            }

            foreach (IFormFile file in files)
            {
                string fileName = _webHostEnvironment.WebRootPath + $@"\Uploaded\{file.FileName}";

                if (!file.ContentType.Equals("text/csv"))
                {
                    string message = "not .csv file";
                    _logger.Information(loggerTemplate + message);

                    return new JsonResult(new
                    {
                        success = false,
                        message
                    });
                }

                if (file.Length > 20480000)
                {
                    string message = "Too big, max 2 mb";
                    _logger.Information(loggerTemplate + message);

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

                    FileStream fs;
                    await using (fs = File.Create(fileName))
                    {
                        await file.CopyToAsync(fs);
                    }

                    TextFieldParser reader;
                    using (reader = new TextFieldParser(new StreamReader(fileName, Encoding.GetEncoding(culture.TextInfo.ANSICodePage), true)))
                    {
                        reader.HasFieldsEnclosedInQuotes = true;
                        reader.SetDelimiters(";");
                        string[] txt = null;

                        for (int i = 0; i < 12; i++)
                        {
                            txt = reader.ReadFields(); // skip 12 lines
                        }

                        StanUpdate lastSuccessful = _unitOfWork.StanUpdate.GetLatestSuccessfulUpdateAsync();
                        DateTime? dateTimeOfData = DateTime.TryParse(txt[4], culture, dateStyle, out DateTime date) ? date : null;

                        if (dateTimeOfData is null)
                        {
                            string message = "Date of data is empty";

                            _logger.Information(loggerTemplate + message);

                            return new JsonResult(new
                            {
                                success = false,
                                message
                            });
                        }

                        if (lastSuccessful is not null && dateTimeOfData <= lastSuccessful.DateOfData)
                        {
                            string message = "Data cannot be older";

                            _logger.Information(loggerTemplate + message);

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

                        StanUpdate stanUpdate = new()
                        {
                            UpdateBegan = DateTime.Now,
                            ExecutedBy = userName,
                        };

                        await _unitOfWork.StanUpdate.Add(stanUpdate);
                        _ = await _unitOfWork.Complete();

                        while (!reader.EndOfData)
                        {
                            try
                            {
                                string[] stanDetailsCsvLine = reader.ReadFields();
                                int startIndex = stanDetailsCsvLine.Length == 16 ? 1 : 2;
                                Stan stan = await _unitOfWork.Stan.FindExact(e => e.StanId == int.Parse(stanDetailsCsvLine[startIndex + 0]));

                                if (stan != null)
                                {
                                    stan.SifraObjekta = int.Parse(stanDetailsCsvLine[startIndex + 1]);
                                    stan.Vrsta = stanDetailsCsvLine[startIndex + 2].Equals("") ? null : stanDetailsCsvLine[startIndex + 2];
                                    stan.Adresa = stanDetailsCsvLine[startIndex + 3].Equals("") ? null : stanDetailsCsvLine[startIndex + 3];
                                    stan.Kat = stanDetailsCsvLine[startIndex + 4].Equals("") ? null : stanDetailsCsvLine[startIndex + 4];
                                    stan.BrojSTana = stanDetailsCsvLine[startIndex + 5].Equals("") ? null : stanDetailsCsvLine[startIndex + 5];
                                    stan.Naselje = stanDetailsCsvLine[startIndex + 6].Equals("") ? null : stanDetailsCsvLine[startIndex + 6];
                                    stan.Četvrt = stanDetailsCsvLine[startIndex + 7].Equals("") ? null : stanDetailsCsvLine[startIndex + 7];
                                    stan.Površina = double.TryParse(stanDetailsCsvLine[startIndex + 8], style, culture, out double d) ? d : null;
                                    stan.StatusKorištenja = stanDetailsCsvLine[startIndex + 9].Equals("") ? null : stanDetailsCsvLine[startIndex + 9];
                                    stan.Korisnik = stanDetailsCsvLine[startIndex + 10].Equals("") ? null : stanDetailsCsvLine[startIndex + 10];
                                    stan.Vlasništvo = stanDetailsCsvLine[startIndex + 11].Equals("") ? null : stanDetailsCsvLine[startIndex + 11];
                                    stan.DioNekretnine = stanDetailsCsvLine[startIndex + 12].Equals("") ? null : stanDetailsCsvLine[startIndex + 12];
                                    stan.Sektor = stanDetailsCsvLine[startIndex + 13].Equals("") ? null : stanDetailsCsvLine[startIndex + 13];
                                    stan.Status = stanDetailsCsvLine[startIndex + 14].Equals("") ? null : stanDetailsCsvLine[startIndex + 14];
                                    _ = await _unitOfWork.Stan.Update(stan);
                                }

                                else
                                {
                                    Stan newStan = new()
                                    {
                                        StanId = int.Parse(stanDetailsCsvLine[startIndex + 0]),
                                        SifraObjekta = int.Parse(stanDetailsCsvLine[startIndex + 1]),
                                        Vrsta = stanDetailsCsvLine[startIndex + 2].Equals("") ? null : stanDetailsCsvLine[startIndex + 2],
                                        Adresa = stanDetailsCsvLine[startIndex + 3].Equals("") ? null : stanDetailsCsvLine[startIndex + 3],
                                        Kat = stanDetailsCsvLine[startIndex + 4].Equals("") ? null : stanDetailsCsvLine[startIndex + 4],
                                        BrojSTana = stanDetailsCsvLine[startIndex + 5].Equals("") ? null : stanDetailsCsvLine[startIndex + 5],
                                        Naselje = stanDetailsCsvLine[startIndex + 6].Equals("") ? null : stanDetailsCsvLine[startIndex + 6],
                                        Četvrt = stanDetailsCsvLine[startIndex + 7].Equals("") ? null : stanDetailsCsvLine[startIndex + 7],
                                        Površina = double.TryParse(stanDetailsCsvLine[startIndex + 8], style, culture, out double d) ? d : null,
                                        StatusKorištenja = stanDetailsCsvLine[startIndex + 9].Equals("") ? null : stanDetailsCsvLine[startIndex + 9],
                                        Korisnik = stanDetailsCsvLine[startIndex + 10].Equals("") ? null : stanDetailsCsvLine[startIndex + 10],
                                        Vlasništvo = stanDetailsCsvLine[startIndex + 11].Equals("") ? null : stanDetailsCsvLine[startIndex + 11],
                                        DioNekretnine = stanDetailsCsvLine[startIndex + 12].Equals("") ? null : stanDetailsCsvLine[startIndex + 12],
                                        Sektor = stanDetailsCsvLine[startIndex + 13].Equals("") ? null : stanDetailsCsvLine[startIndex + 13],
                                        Status = stanDetailsCsvLine[startIndex + 14].Equals("") ? null : stanDetailsCsvLine[startIndex + 14],
                                    };

                                    await _unitOfWork.Stan.Add(newStan);
                                    _ = await _unitOfWork.Complete();

                                    //_logger.Information(_loggerTemplate + "Added: " + newStan.StanId);
                                }

                            }
                            catch (Exception e)
                            {
                                _logger.Error(loggerTemplate + e.Message);

                                stanUpdate.Interrupted = true;
                                _ = await _unitOfWork.StanUpdate.Update(stanUpdate);
                            }
                        }

                        stanUpdate.UpdateEnded = DateTime.Now;
                        stanUpdate.UpdateComplete = true;
                        stanUpdate.DateOfData = (DateTime)dateTimeOfData;
                        _ = await _unitOfWork.StanUpdate.Update(stanUpdate);

                    }

                }

                catch (Exception e)
                {
                    _logger.Error(loggerTemplate + e.Message);

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
                _logger.Information(loggerTemplate + message);

                return new JsonResult(new
                {
                    success = true,
                    message
                });
            }
        }
    }
}
