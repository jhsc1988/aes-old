using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.RacuniHoldingService.IService
{
    public interface IRacuniHoldingTempCreateService
    {
        Task<JsonResult> AddNewTemp(string brojRacuna, string iznos, string datumIzdavanja, string userId);
        Task<int> CheckTempTableForRacuniWithouCustomer(string userId);
        Task<JsonResult> RefreshCustomers(string userId);
    }
}