using aes.Models.Racuni.Elektra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.Elektra.RacuniElektraIzvrsenjeUsluge.Is
{
    public interface IRacuniElektraIzvrsenjeUslugeService : IRacuniService.IRacuniervice
    {
        Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetCreateRacuni(string userId);
        Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetList(int predmetIdAsInt, int dopisIdAsInt);
    }
}