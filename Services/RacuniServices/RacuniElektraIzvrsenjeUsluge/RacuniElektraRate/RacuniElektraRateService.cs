using aes.CommonDependecies;
using aes.Models.Racuni;
using aes.Services.RacuniServices.RacuniElektraIzvrsenjeUsluge.RacuniElektraRate.Is;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.RacuniElektraIzvrsenjeUsluge.RacuniElektraRate
{
    public class RacuniElektraRateService : Racuniervice, IRacuniElektraRateService
    {
        private readonly IRacuniCommonDependecies _c;

        public RacuniElektraRateService(IRacuniCommonDependecies c)
        {
            _c = c;
        }

        public async Task<IEnumerable<RacunElektraRate>> GetList(int predmetIdAsInt, int dopisIdAsInt)
        {
            return await _c.UnitOfWork.RacuniElektraRate.GetRacuni(predmetIdAsInt, dopisIdAsInt);
        }

        public async Task<IEnumerable<RacunElektraRate>> GetCreateRacuni(string userId)
        {
            return await _c.UnitOfWork.RacuniElektraRate.TempList(userId);
        }
    }
}
