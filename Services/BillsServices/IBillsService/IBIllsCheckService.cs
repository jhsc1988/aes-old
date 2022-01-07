using aes.Models.Racuni;
using System.Collections.Generic;

namespace aes.Services.BillsServices.IBillsService
{
    public interface IBillsCheckService
    {
        string CheckIfExistsInPayed(string brojRacuna, IEnumerable<Racun> bills);
        string CheckIfExistsInTemp(string brojRacuna, IEnumerable<Racun> tempBills);
    }
}
