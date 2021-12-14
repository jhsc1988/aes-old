using aes.CommonDependecies;
using aes.Models.Racuni;
using aes.Services.BillsServices.BillsHoldingService.IService;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace aes.Services.BillsServices.BillsHoldingService
{
    public class BillsHoldingService : BillService, IBillsHoldingService
    {
        private readonly IBillsCommonDependecies _c;

        public BillsHoldingService(IBillsCommonDependecies c)
        {
            _c = c;
        }

        public async Task<IEnumerable<RacunHolding>> GetList(int predmetIdAsInt, int dopisIdAsInt)
        {
            return await _c.UnitOfWork.BillsHolding.GetBills(predmetIdAsInt, dopisIdAsInt);
        }

        public async Task<IEnumerable<RacunHolding>> GetCreateBills(string userId)
        {
            return await _c.UnitOfWork.BillsHolding.TempList(userId);
        }
    }
}
