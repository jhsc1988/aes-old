using aes.CommonDependecies;
using aes.Models.Racuni;
using aes.Services.BillsServices.BillsElektraServices.BillsElektraAdvances.Is;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.BillsElektraServices.BillsElektraAdvances
{
    public class BillsElektraAdvancesService : BillService, IBillsElektraAdvancesService
    {
        private readonly IBillsCommonDependecies _c;

        public BillsElektraAdvancesService(IBillsCommonDependecies c)
        {
            _c = c;
        }

        public async Task<IEnumerable<RacunElektraRate>> GetList(int predmetIdAsInt, int dopisIdAsInt)
        {
            return await _c.UnitOfWork.BillsElektraAdvances.GetRacuni(predmetIdAsInt, dopisIdAsInt);
        }

        public async Task<IEnumerable<RacunElektraRate>> GetCreateBills(string userId)
        {
            return await _c.UnitOfWork.BillsElektraAdvances.TempList(userId);
        }
    }
}
