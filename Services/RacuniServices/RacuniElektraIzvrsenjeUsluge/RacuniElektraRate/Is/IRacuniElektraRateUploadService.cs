using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.RacuniElektraIzvrsenjeUsluge.RacuniElektraRate.Is
{
    public interface IRacuniElektraRateUploadService
    {
        Task<JsonResult> Upload(HttpRequest Request, string userId);
    }
}