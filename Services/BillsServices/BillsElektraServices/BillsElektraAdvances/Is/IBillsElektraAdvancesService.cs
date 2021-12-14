using aes.Models.Racuni;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.BillsElektraServices.BillsElektraAdvances.Is
{
    public interface IBillsElektraAdvancesService : IBillsService.IBillService
    {
        Task<IEnumerable<RacunElektraRate>> GetCreateBills(string userId);
        Task<IEnumerable<RacunElektraRate>> GetList(int predmetIdAsInt, int dopisIdAsInt);
    }
}