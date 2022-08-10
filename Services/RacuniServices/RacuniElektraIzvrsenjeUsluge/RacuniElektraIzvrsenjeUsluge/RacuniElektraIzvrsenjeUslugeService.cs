using aes.CommonDependecies;
using aes.Models.Racuni;
using aes.Services.RacuniServices.RacuniElektraIzvrsenjeUsluge.RacuniElektraIzvrsenjeUsluge.Is;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.RacuniElektraIzvrsenjeUsluge.RacuniElektraIzvrsenjeUsluge
{
    public class RacuniElektraIzvrsenjeUslugeService : Racuniervice, IRacuniElektraIzvrsenjeUslugeService
    {
        private readonly IRacuniCommonDependecies _c;

        public RacuniElektraIzvrsenjeUslugeService(IRacuniCommonDependecies c)
        {
            _c = c;
        }

        public async Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetList(int predmetIdAsInt, int dopisIdAsInt)
        {
            return await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.GetRacuni(predmetIdAsInt, dopisIdAsInt);
        }

        public async Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetCreateRacuni(string userId)
        {
            return await _c.UnitOfWork.RacuniElektraIzvrsenjeUsluge.TempList(userId);
        }
    }
}
