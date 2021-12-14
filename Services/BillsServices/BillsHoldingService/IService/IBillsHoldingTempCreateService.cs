using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.BillsHoldingService.IService
{
    public interface IBillsHoldingTempCreateService
    {
        Task<JsonResult> AddNewTemp(string brojRacuna, string iznos, string datumIzdavanja, string userId);
        Task<int> CheckTempTableForBillsWithouCustomer(string userId);
        Task<JsonResult> RefreshCustomers(string userId);
    }
}