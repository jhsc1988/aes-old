using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.RacuniHoldingService.IService
{
    public interface IRacuniHoldingUploadService
    {
        Task<JsonResult> Upload(HttpRequest Request, string userId);
    }
}