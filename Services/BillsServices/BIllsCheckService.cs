using aes.Models.Racuni;
using aes.Services.BillsServices.IBillsService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aes.Services.BillsServices
{
    public class BillsCheckService : IBillsCheckService
    {
        public string CheckIfExistsInTemp(string brojRacuna, IEnumerable<Racun> tempBills)
        {

            int numOfOccurrences = tempBills.Count(x => x.BrojRacuna.Equals(brojRacuna, StringComparison.InvariantCultureIgnoreCase));
            return numOfOccurrences >= 2 ? "dupli račun" : null;
        }
        public string CheckIfExistsInPayed(string brojRacuna, IEnumerable<Racun> bills)
        {

            int numOfOccurrences = bills.Count(x => x.BrojRacuna.Equals(brojRacuna, StringComparison.InvariantCultureIgnoreCase));
            return numOfOccurrences >= 1 ? "račun već plaćen" : null;


        }
    }
}
