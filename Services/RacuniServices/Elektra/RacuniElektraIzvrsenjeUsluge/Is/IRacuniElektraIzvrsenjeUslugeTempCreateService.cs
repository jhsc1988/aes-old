using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.Elektra.RacuniElektraIzvrsenjeUsluge.Is
{
    public interface IRacuniElektraIzvrsenjeUslugeTempCreateService
    {
        Task<JsonResult> AddNewTemp(string brojRacuna, string iznos, string datumPotvrde, string datumIzvrsenja, string usluga, string dopisId, string userId);
        Task<int> CheckTempTableForRacuniWithousElectraCustomer(string userId);
        Task<JsonResult> RefreshCustomers(string userId);
    }
}