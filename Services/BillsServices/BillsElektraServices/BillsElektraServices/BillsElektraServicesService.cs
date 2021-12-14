using aes.CommonDependecies;
using aes.Models.Racuni;
using aes.Services.BillsServices.BillsElektraServices.BillsElektraServices.Is;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.BillsElektraServices.BillsElektraServices
{
    public class BillsElektraServicesService : BillService, IBillsElektraServicesService
    {
        private readonly IBillsCommonDependecies _c;

        public BillsElektraServicesService(IBillsCommonDependecies c)
        {
            _c = c;
        }

        public async Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetList(int predmetIdAsInt, int dopisIdAsInt)
        {
            return await _c.UnitOfWork.BillsElektraServices.GetRacuni(predmetIdAsInt, dopisIdAsInt);
        }

        public async Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetCreateBills(string userId)
        {
            return await _c.UnitOfWork.BillsElektraServices.TempList(userId);
        }
    }
}
