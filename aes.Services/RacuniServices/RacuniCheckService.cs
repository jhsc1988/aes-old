using aes.Models.Racuni;
using aes.Services.RacuniServices.IRacuniService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aes.Services.RacuniServices
{
    public class RacuniCheckService : IRacuniCheckService
    {
        public string CheckIfExistsInTemp(string brojRacuna, IEnumerable<Racun> tempRacuni)
        {

            int numOfOccurrences = tempRacuni.Count(x => x.BrojRacuna.Equals(brojRacuna, StringComparison.InvariantCultureIgnoreCase));
            return numOfOccurrences >= 2 ? "dupli račun" : null;
        }
        public string CheckIfExistsInPayed(string brojRacuna, IEnumerable<Racun> Racuni)
        {

            int numOfOccurrences = Racuni.Count(x => x.BrojRacuna.Equals(brojRacuna, StringComparison.InvariantCultureIgnoreCase));
            return numOfOccurrences >= 1 ? "račun već plaćen" : null;


        }
    }
}
