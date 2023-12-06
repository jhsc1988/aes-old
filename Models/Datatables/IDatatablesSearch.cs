using aes.Models.HEP;
using aes.Models.Racuni.Elektra;
using aes.Models.Racuni.Holding;
using System.Collections.Generic;

namespace aes.Models.Datatables
{
    public interface IDatatablesSearch
    {
        IEnumerable<RacunElektraIzvrsenjeUsluge> GetRacunElektraIzvrsenjeUslugeForDatatables(IEnumerable<RacunElektraIzvrsenjeUsluge> CreateRacuniElektraIzvrsenjeUslugeList, DtParams dtParams);
        IEnumerable<RacunElektra> GetRacuniElektraForDatatables(IEnumerable<RacunElektra> CreateRacuniElektraList, DtParams dtParams);
        IEnumerable<RacunElektraRate> GetRacuniElektraRateForDatatables(IEnumerable<RacunElektraRate> CreateRacuniElektraRateList, DtParams dtParams);
        IEnumerable<RacunHolding> GetRacuniHoldingForDatatables(IEnumerable<RacunHolding> CreateRRacuniHoldingList, DtParams dtParams);
        IEnumerable<Stan> GetStanoviForDatatables(IEnumerable<Stan> stanList, DtParams dtParams);
        IEnumerable<Ods> GetStanoviOdsForDatatables(IEnumerable<Ods> OdsList, DtParams dtParams);
        IEnumerable<Predmet> GetPredmetiForDatatables(IEnumerable<Predmet> predmetList, DtParams Params);
        IEnumerable<Dopis> GetDopisiForDatatables(IEnumerable<Dopis> DopisList, DtParams Params);
        IEnumerable<ElektraKupac> GetElektraKupciForDatatables(IEnumerable<ElektraKupac> ElektraKupacList, DtParams Params);
        IEnumerable<ObracunPotrosnje> GetObracunPotrosnjeDatatables(IEnumerable<ObracunPotrosnje> list, DtParams Params);
    }
}