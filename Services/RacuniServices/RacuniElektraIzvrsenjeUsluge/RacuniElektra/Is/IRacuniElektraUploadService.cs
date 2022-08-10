using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.RacuniElektraIzvrsenjeUsluge.RacuniElektra.Is
{
    public interface IRacuniElektraUploadService
    {
        Task<JsonResult> Upload(HttpRequest Request, string userId);
    }
}