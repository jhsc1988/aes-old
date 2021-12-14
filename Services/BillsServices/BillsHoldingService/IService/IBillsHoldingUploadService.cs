using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.BillsHoldingService.IService
{
    public interface IBillsHoldingUploadService
    {
        Task<JsonResult> Upload(HttpRequest Request, string userId);
    }
}