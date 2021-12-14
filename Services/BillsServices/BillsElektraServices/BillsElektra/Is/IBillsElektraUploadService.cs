using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.BillsElektraServices.BillsElektra.Is
{
    public interface IBillsElektraUploadService
    {
        Task<JsonResult> Upload(HttpRequest Request, string userId);
    }
}