using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.BillsElektraServices.BillsElektraAdvances.Is
{
    public interface IBillsElektraAdvancesTempCreateService
    {
        Task<JsonResult> AddNewTemp(string brojRacuna, string iznos, string razdoblje, string userId);
        Task<int> CheckTempTableForBillsWithousElectraCustomer(string userId);
        Task<JsonResult> RefreshCustomers(string userId);
    }
}