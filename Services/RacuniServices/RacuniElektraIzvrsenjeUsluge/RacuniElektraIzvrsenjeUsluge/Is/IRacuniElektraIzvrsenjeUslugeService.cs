using aes.Models.Racuni;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.RacuniElektraIzvrsenjeUsluge.RacuniElektraIzvrsenjeUsluge.Is
{
    public interface IRacuniElektraIzvrsenjeUslugeService : IRacuniService.IRacuniervice
    {
        Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetCreateRacuni(string userId);
        Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetList(int predmetIdAsInt, int dopisIdAsInt);
    }
}