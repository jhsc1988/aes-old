using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.IServices
{
    public interface IStanUploadService
    {
        Task<JsonResult> Upload(HttpRequest Request, string userName);
    }
}