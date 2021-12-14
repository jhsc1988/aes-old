using aes.CommonDependecies;
using aes.Models.Racuni;
using aes.Services.BillsServices.BillsElektraServices.BillsElektra.Is;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.BillsElektraServices.BillsElektra
{
    public class BillsElektraService : BillService, IBillsElektraService
    {
        private readonly IBillsCommonDependecies _c;

        public BillsElektraService(IBillsCommonDependecies c)
        {
            _c = c;
        }

        public async Task<IEnumerable<RacunElektra>> GetList(int predmetIdAsInt, int dopisIdAsInt)
        {
            return await _c.UnitOfWork.BillsElektra.GetRacuni(predmetIdAsInt, dopisIdAsInt);
        }

        public async Task<IEnumerable<RacunElektra>> GetCreateBills(string userId)
        {
            return await _c.UnitOfWork.BillsElektra.TempList(userId);
        }
    }
}
