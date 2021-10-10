using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public interface IElektraKupacWorkshop
    {
        List<RacunElektra> GetRacuniForKupac(int param);
        List<RacunElektraRate> GetRacuniRateForKupac(int param);
        List<RacunElektraIzvrsenjeUsluge> GetRacuniElektraIzvrsenjeForKupac(int param);
        List<ElektraKupac> GetKupciForDatatables(DatatablesParams Params, List<ElektraKupac> ElektraKupacList);
    }
}
