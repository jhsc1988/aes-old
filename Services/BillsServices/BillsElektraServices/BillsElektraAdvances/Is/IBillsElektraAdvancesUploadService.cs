using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.BillsElektraServices.BillsElektraAdvances.Is
{
    public interface IBillsElektraAdvancesUploadService
    {
        Task<JsonResult> Upload(HttpRequest Request, string userId);
    }
}