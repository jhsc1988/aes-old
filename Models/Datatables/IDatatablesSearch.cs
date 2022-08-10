using aes.Models.Racuni;
using System.Collections.Generic;

namespace aes.Models.Datatables
{
    public interface IDatatablesSearch
    {
        IEnumerable<RacunElektraIzvrsenjeUsluge> GetRacunElektraIzvrsenjeUslugeForDatatables(IEnumerable<RacunElektraIzvrsenjeUsluge> CreateRacuniElektraIzvrsenjeUslugeList, DTParams dtParams);
        IEnumerable<RacunElektra> GetRacuniElektraForDatatables(IEnumerable<RacunElektra> CreateRacuniElektraList, DTParams dtParams);
        IEnumerable<RacunElektraRate> GetRacuniElektraRateForDatatables(IEnumerable<RacunElektraRate> CreateRacuniElektraRateList, DTParams dtParams);
        IEnumerable<RacunHolding> GetRacuniHoldingForDatatables(IEnumerable<RacunHolding> CreateRRacuniHoldingList, DTParams dtParams);
        IEnumerable<Stan> GetStanoviForDatatables(IEnumerable<Stan> stanList, DTParams dtParams);
        IEnumerable<Ods> GetStanoviOdsForDatatables(IEnumerable<Ods> OdsList, DTParams dtParams);
        IEnumerable<Predmet> GetPredmetiForDatatables(IEnumerable<Predmet> predmetList, DTParams Params);
        IEnumerable<Dopis> GetDopisiForDatatables(IEnumerable<Dopis> DopisList, DTParams Params);
        IEnumerable<ElektraKupac> GetElektraKupciForDatatables(IEnumerable<ElektraKupac> ElektraKupacList, DTParams Params);
        IEnumerable<ObracunPotrosnje> GetObracunPotrosnjeDatatables(IEnumerable<ObracunPotrosnje> list, DTParams Params);
    }
}