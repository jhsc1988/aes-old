using aes.CommonDependecies.ICommonDependencies;
using aes.Models.Racuni.Elektra;
using aes.Services.RacuniServices.Elektra.RacuniElektra.Is;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.Elektra.RacuniElektra
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
