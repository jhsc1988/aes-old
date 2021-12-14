using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.BillsElektraServices.BillsElektraServices.Is
{
    public interface IBillsElektraServicesTempCreateService
    {
        Task<JsonResult> AddNewTemp(string brojRacuna, string iznos, string datumPotvrde, string datumIzvrsenja, string usluga, string dopisId, string userId);
        Task<int> CheckTempTableForBillsWithousElectraCustomer(string userId);
        Task<JsonResult> RefreshCustomers(string userId);
    }
}