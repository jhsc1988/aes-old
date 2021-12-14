using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.IServices
{
    public interface IOdsService
    {
        Task<JsonResult> GetStanData(string sid);
        Task<JsonResult> GetStanDataForKupci(string OdsId);
    }
}