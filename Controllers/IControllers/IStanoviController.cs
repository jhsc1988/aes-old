using Microsoft.AspNetCore.Mvc;

namespace aes.Controllers.IControllers
{
    internal interface IStanoviController
    {
        public Task<JsonResult> GetList();
        public Task<JsonResult> GetListFiltered();
        [HttpPost]
        Task<JsonResult> GetRacuniHoldingForStan(int param);
        [HttpPost]
        Task<JsonResult> GetRacuniElektraForStan(int param);
        [HttpPost]
        Task<JsonResult> GetRacuniElektraRateForStan(int param);
        [HttpPost]
        Task<JsonResult> GetRacuniElektraIzvrsenjeUslugeForStan(int param);
    }
}
