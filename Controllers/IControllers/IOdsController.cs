using Microsoft.AspNetCore.Mvc;

namespace aes.Controllers.IControllers
{
    internal interface IOdsController
    {
        public Task<IActionResult> GetList();
        Task<JsonResult> GetStanData(string sid);
        Task<JsonResult> GetStanDataForOmm(string OdsId);
    }
}
