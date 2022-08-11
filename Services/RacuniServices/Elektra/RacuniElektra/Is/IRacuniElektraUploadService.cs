using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.Elektra.RacuniElektra.Is
{
    public interface IRacuniElektraUploadService
    {
        Task<JsonResult> Upload(HttpRequest Request, string userId);
    }
}