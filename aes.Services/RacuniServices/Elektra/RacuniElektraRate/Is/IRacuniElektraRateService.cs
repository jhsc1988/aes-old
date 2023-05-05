using aes.Models.Racuni.Elektra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.Elektra.RacuniElektraRate.Is
{
    public interface IRacuniElektraRateService : IRacuniService.IRacuniervice
    {
        Task<IEnumerable<RacunElektraRate>> GetCreateRacuni(string userId);
        Task<IEnumerable<RacunElektraRate>> GetList(int predmetIdAsInt, int dopisIdAsInt);
    }
}