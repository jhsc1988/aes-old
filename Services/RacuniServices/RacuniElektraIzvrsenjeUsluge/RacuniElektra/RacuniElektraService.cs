using aes.CommonDependecies;
using aes.Models.Racuni;
using aes.Services.RacuniServices.RacuniElektraIzvrsenjeUsluge.RacuniElektra.Is;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.RacuniElektraIzvrsenjeUsluge.RacuniElektra
{
    public class RacuniElektraService : Racuniervice, IRacuniElektraService
    {
        private readonly IRacuniCommonDependecies _c;

        public RacuniElektraService(IRacuniCommonDependecies c)
        {
            _c = c;
        }

        public async Task<IEnumerable<RacunElektra>> GetList(int predmetIdAsInt, int dopisIdAsInt)
        {
            return await _c.UnitOfWork.RacuniElektra.GetRacuni(predmetIdAsInt, dopisIdAsInt);
        }

        public async Task<IEnumerable<RacunElektra>> GetCreateRacuni(string userId)
        {
            return await _c.UnitOfWork.RacuniElektra.TempList(userId);
        }
    }
}
