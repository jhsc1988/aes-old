using aes.Models.Racuni.Elektra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.Elektra.RacuniElektra.Is
{
    public interface IRacuniElektraService : IRacuniService.IRacuniervice
    {
        Task<IEnumerable<RacunElektra>> GetCreateRacuni(string userId);
        Task<IEnumerable<RacunElektra>> GetList(int predmetIdAsInt, int dopisIdAsInt);
    }
}