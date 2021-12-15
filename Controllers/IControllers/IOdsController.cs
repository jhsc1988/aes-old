using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Controllers.IControllers
{
    internal interface IOdsController
    {
        public Task<IActionResult> GetList();
        Task<JsonResult> GetStanData(string sid);
        Task<JsonResult> GetStanDataForOmm(string OdsId);
    }
}
