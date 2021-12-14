using aes.Models.Racuni;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.BillsElektraServices.BillsElektra.Is
{
    public interface IBillsElektraService : IBillsService.IBillService
    {
        Task<IEnumerable<RacunElektra>> GetCreateBills(string userId);
        Task<IEnumerable<RacunElektra>> GetList(int predmetIdAsInt, int dopisIdAsInt);
    }
}