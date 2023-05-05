using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.Elektra.RacuniElektra.Is
{
    public interface IRacuniElektraTempCreateService
    {
        Task<JsonResult> AddNewTemp(string brojRacuna, string iznos, string datumIzdavanja, string userId);
        Task<int> CheckTempTableForRacuniWithousElektraKupac(string userId);
        Task<JsonResult> RefreshCustomers(string userId);
    }
}