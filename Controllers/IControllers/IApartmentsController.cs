using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Controllers.IControllers
{
    internal interface IApartmentsController
    {
        public Task<JsonResult> GetList();
        public Task<JsonResult> GetListFiltered();
        [HttpPost]
        Task<JsonResult> GetBillsHoldingForApartment(int param);
        [HttpPost]
        Task<JsonResult> GetBillsElektraForApartment(int param);
        [HttpPost]
        Task<JsonResult> GetBillsElektraAdvancesForApartment(int param);
        [HttpPost]
        Task<JsonResult> GetBillsElektraServicesForApartment(int param);
    }
}
