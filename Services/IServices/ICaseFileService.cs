using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.IServices
{
    public interface ICaseFileService
    {
        Task<JsonResult> SaveToDB(string klasa, string naziv);
    }
}