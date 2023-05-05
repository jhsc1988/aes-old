using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.IServices
{
    public interface IPredmetiervice
    {
        Task<JsonResult> SaveToDB(string klasa, string naziv);
    }
}