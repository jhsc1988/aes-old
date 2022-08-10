using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.RacuniElektraIzvrsenjeUsluge.RacuniElektraRate.Is
{
    public interface IRacuniElektraRateTempCreateService
    {
        Task<JsonResult> AddNewTemp(string brojRacuna, string iznos, string razdoblje, string userId);
        Task<int> CheckTempTableForRacuniWithousElectraCustomer(string userId);
        Task<JsonResult> RefreshCustomers(string userId);
    }
}