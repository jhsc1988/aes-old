using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.BillsElektraServices.BillsElektra.Is
{
    public interface IBillsElektraTempCreateService
    {
        Task<JsonResult> AddNewTemp(string brojRacuna, string iznos, string datumIzdavanja, string userId);
        Task<int> CheckTempTableForBillsWithousElektraCustomer(string userId);
        Task<JsonResult> RefreshCustomers(string userId);
    }
}