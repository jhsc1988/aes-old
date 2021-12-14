using aes.Models.Racuni;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.IBillsService
{
    public interface IBillsCheckService
    {
        Task<string> CheckIfExistsInPayed(string brojRacuna, IEnumerable<Racun> bills);
        Task<string> CheckIfExistsInTemp(string brojRacuna, IEnumerable<Racun> tempBills);
    }
}
