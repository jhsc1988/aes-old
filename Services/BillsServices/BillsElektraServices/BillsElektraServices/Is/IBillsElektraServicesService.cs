using aes.Models.Racuni;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.BillsElektraServices.BillsElektraServices.Is
{
    public interface IBillsElektraServicesService : IBillsService.IBillService
    {
        Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetCreateBills(string userId);
        Task<IEnumerable<RacunElektraIzvrsenjeUsluge>> GetList(int predmetIdAsInt, int dopisIdAsInt);
    }
}