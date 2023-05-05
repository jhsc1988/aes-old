using aes.CommonDependecies.ICommonDependencies;
using aes.Models.Racuni.Holding;
using aes.Services.RacuniServices.RacuniHoldingService.IService;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace aes.Services.RacuniServices.RacuniHoldingService
{
    public class RacuniHoldingService : Racuniervice, IRacuniHoldingService
    {
        private readonly IRacuniCommonDependecies _c;

        public RacuniHoldingService(IRacuniCommonDependecies c)
        {
            _c = c;
        }

        public async Task<IEnumerable<RacunHolding>> GetList(int predmetIdAsInt, int dopisIdAsInt)
        {
            return await _c.UnitOfWork.RacuniHolding.GetRacuni(predmetIdAsInt, dopisIdAsInt);
        }

        public async Task<IEnumerable<RacunHolding>> GetCreateRacuni(string userId)
        {
            return await _c.UnitOfWork.RacuniHolding.TempList(userId);
        }
    }
}
