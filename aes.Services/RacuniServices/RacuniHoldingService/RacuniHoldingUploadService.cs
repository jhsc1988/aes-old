using aes.Services.RacuniServices.RacuniHoldingService.IService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using aes.UnitOfWork;

namespace aes.Services.RacuniServices.RacuniHoldingService
{
    public class RacuniHoldingUploadService : IRacuniHoldingUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRacuniHoldingTempCreateService _RacuniHoldingTempCreateService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;


        public RacuniHoldingUploadService(IWebHostEnvironment webHostEnvironment, IRacuniHoldingTempCreateService RacuniHoldingTempCreateService,
            IUnitOfWork unitOfWork, ILogger logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _RacuniHoldingTempCreateService = RacuniHoldingTempCreateService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<JsonResult> Upload(HttpRequest Request, string userId)
        {
            string _loggerTemplate = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + ", " + "UserId: " + userId + ", " + "msg: ";

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
                StreamReader reader;
                string fileName = _webHostEnvironment.WebRootPath + $@"\Uploaded\{file.FileName}";

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

                if (file.Length > 256000)
                {
                    string message = "Too big, max 256 kb";
                    _logger.Information(_loggerTemplate + message);

                    return new JsonResult(new
                    {
                        success = false,
                        message
                    });
                }
                try
                {
                    using (fs = File.Create(fileName))
                    {
                        file.CopyTo(fs);
                    }
                    using (reader = new StreamReader(fileName))
                    {
                        double iznos = 0;

                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            if (line.Contains("EUR"))
                            {
                                iznos = double.Parse(reader.ReadLine()) / 100;
                                for (int i = 0; i < 3; i++)
                                {
                                    _ = reader.ReadLine();
                                }
                                line = reader.ReadLine();
                                if (!line.Contains("ZAGREBAÄ\u008dKI HOLDING D.O.O."))
                                {
                                    throw new Exception("Nije račun Zagrebačkog holdinga");
                                }
                                for (int i = 0; i < 3; i++) // skipping 3 lines
                                {
                                    _ = reader.ReadLine();
                                }
                                line = reader.ReadLine();
                            }
                            if (line.Contains("HR01"))
                            {
                                if ((await _unitOfWork.RacuniHolding.TempList(userId)).Count() >= 500)
                                {
                                    string message = "U tablici ne može biti više od 500 računa!";
                                    _logger.Information(_loggerTemplate + message);

                                    return new(new { success = false, Message = message });
                                }

                                _ = await _RacuniHoldingTempCreateService.AddNewTemp(reader.ReadLine(), iznos.ToString(), null, userId);
                                for (int i = 0; i < 4; i++) // skipping 4 lines
                                {
                                    _ = reader.ReadLine();
                                }
                            }
                        }
                    };
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

