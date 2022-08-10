using aes.Models.Racuni;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.RacuniElektraIzvrsenjeUsluge.RacuniElektraRate.Is
{
    public interface IRacuniElektraRateService : IRacuniService.IRacuniervice
    {
        Task<IEnumerable<RacunElektraRate>> GetCreateRacuni(string userId);
        Task<IEnumerable<RacunElektraRate>> GetList(int predmetIdAsInt, int dopisIdAsInt);
    }
}