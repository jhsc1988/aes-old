using aes.Models.Racuni;
using System.Collections.Generic;
using System.Linq;

namespace aes.Models.Datatables
{
    public class DatatablesSearch : IDatatablesSearch
    {
        public IEnumerable<RacunElektra> GetRacuniElektraForDatatables(IEnumerable<RacunElektra> CreateRacuniElektraList, DTParams dtParams)
        {
            return CreateRacuniElektraList.Where(
                   x => x.BrojRacuna.Contains(dtParams.SearchValue)
                        || (x.ElektraKupac != null && x.ElektraKupac.Ods != null && x.ElektraKupac.Ods.Stan != null && x.ElektraKupac.Ods.Stan.StanId.ToString().Contains(dtParams.SearchValue))
                        || (x.ElektraKupac != null && x.ElektraKupac.Ods != null && x.ElektraKupac.Ods.Stan.Adresa != null && x.ElektraKupac.Ods.Stan.Adresa.ToLower().Contains(dtParams.SearchValue.ToLower()))
                        || (x.ElektraKupac != null && x.ElektraKupac.Ods != null && x.ElektraKupac.Ods.Stan.Korisnik != null && x.ElektraKupac.Ods.Stan.Korisnik.ToLower().Contains(dtParams.SearchValue.ToLower()))
                        || (x.ElektraKupac != null && x.ElektraKupac.Ods != null && x.ElektraKupac.Ods.Stan.Vlasništvo != null && x.ElektraKupac.Ods.Stan.Vlasništvo.ToLower().Contains(dtParams.SearchValue.ToLower()))
                        || (x.ElektraKupac != null && x.ElektraKupac.Ods != null && x.ElektraKupac.Ods.Stan.StatusKorištenja != null && x.ElektraKupac.Ods.Stan.StatusKorištenja.ToLower().Contains(dtParams.SearchValue.ToLower()))
                        || (x.ElektraKupac != null && x.ElektraKupac.UgovorniRacun.ToString().Contains(dtParams.SearchValue))
                        || (x.DatumIzdavanja != null && x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(dtParams.SearchValue))
                        || x.Iznos.ToString().Contains(dtParams.SearchValue)
                        || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(dtParams.SearchValue))
                        || (x.DatumPotvrde != null &&
                            x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(dtParams.SearchValue))
                        || (x.Napomena != null && x.Napomena.ToLower().Contains(dtParams.SearchValue.ToLower())));

        }

        public IEnumerable<RacunHolding> GetRacuniHoldingForDatatables(IEnumerable<RacunHolding> CreateRRacuniHoldingList, DTParams dtParams)
        {
            return CreateRRacuniHoldingList.Where(
                x => x.BrojRacuna.Contains(dtParams.SearchValue)
                || x.Stan.SifraObjekta.ToString().Contains(dtParams.SearchValue)
                || x.Stan.StanId.ToString().Contains(dtParams.SearchValue)
                || (x.DatumIzdavanja != null && x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(dtParams.SearchValue))
                || x.Iznos.ToString().Contains(dtParams.SearchValue)
                || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(dtParams.SearchValue))
                || (x.DatumPotvrde != null && x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(dtParams.SearchValue))
                || (x.Napomena != null && x.Napomena.ToLower().Contains(dtParams.SearchValue.ToLower())));
        }
        public IEnumerable<RacunElektraRate> GetRacuniElektraRateForDatatables(IEnumerable<RacunElektraRate> CreateRacuniElektraRateList, DTParams dtParams)
        {
            return CreateRacuniElektraRateList
                .Where(
                    x => x.BrojRacuna.Contains(dtParams.SearchValue)
                    || x.ElektraKupac.UgovorniRacun.ToString().Contains(dtParams.SearchValue)
                    || (x.Razdoblje != null && x.Razdoblje.Value.ToString("MM.yyyy").Contains(dtParams.SearchValue))
                    || x.Iznos.ToString().Contains(dtParams.SearchValue)
                    || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(dtParams.SearchValue))
                    || (x.DatumPotvrde != null && x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(dtParams.SearchValue))
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(dtParams.SearchValue.ToLower())));
        }

        public IEnumerable<RacunElektraIzvrsenjeUsluge> GetRacunElektraIzvrsenjeUslugeForDatatables(IEnumerable<RacunElektraIzvrsenjeUsluge> CreateRacuniElektraIzvrsenjeUslugeList, DTParams dtParams)
        {
            return CreateRacuniElektraIzvrsenjeUslugeList
            .Where(
                x => x.BrojRacuna.Contains(dtParams.SearchValue)
                || x.ElektraKupac.UgovorniRacun.ToString().Contains(dtParams.SearchValue)
                || (x.DatumIzdavanja != null && x.DatumIzdavanja.Value.ToString("dd.MM.yyyy").Contains(dtParams.SearchValue))
                || x.DatumIzvrsenja.Value.ToString("dd.MM.yyyy").Contains(dtParams.SearchValue)
                || (x.Usluga != null && x.Usluga.ToLower().Contains(dtParams.SearchValue.ToLower()))
                || x.Iznos.ToString().Contains(dtParams.SearchValue)
                || (x.KlasaPlacanja != null && x.KlasaPlacanja.Contains(dtParams.SearchValue))
                || (x.DatumPotvrde != null && x.DatumPotvrde.Value.ToString("dd.MM.yyyy").Contains(dtParams.SearchValue))
                || (x.Napomena != null && x.Napomena.ToLower().Contains(dtParams.SearchValue)));
        }

        public IEnumerable<Ods> GetStanoviOdsForDatatables(IEnumerable<Ods> OdsList, DTParams dtParams)
        {
            return OdsList
                .Where(
                    x => x.Omm.ToString().Contains(dtParams.SearchValue)
                    || x.Stan.StanId.ToString().Contains(dtParams.SearchValue)
                    || x.Stan.SifraObjekta.ToString().Contains(dtParams.SearchValue)
                    || (x.Stan.Adresa != null && x.Stan.Adresa.ToLower().Contains(dtParams.SearchValue.ToLower()))
                    || (x.Stan.Kat != null && x.Stan.Kat.ToLower().Contains(dtParams.SearchValue.ToLower()))
                    || (x.Stan.BrojSTana != null && x.Stan.BrojSTana.ToLower().Contains(dtParams.SearchValue.ToLower()))
                    || (x.Stan.Četvrt != null && x.Stan.Četvrt.ToLower().Contains(dtParams.SearchValue.ToLower()))
                    || x.Stan.Površina.ToString().Contains(dtParams.SearchValue)
                    || (x.Stan.StatusKorištenja != null && x.Stan.StatusKorištenja.ToLower().Contains(dtParams.SearchValue.ToLower()))
                    || (x.Stan.Korisnik != null && x.Stan.Korisnik.ToLower().Contains(dtParams.SearchValue.ToLower()))
                    || (x.Stan.Vlasništvo != null && x.Stan.Vlasništvo.ToLower().Contains(dtParams.SearchValue.ToLower()))
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(dtParams.SearchValue.ToLower())));
        }
        public IEnumerable<Stan> GetStanoviForDatatables(IEnumerable<Stan> stanList, DTParams dtParams)
        {
            return stanList
                .Where(
                    x => x.StanId.ToString().Contains(dtParams.SearchValue)
                        || x.SifraObjekta.ToString().Contains(dtParams.SearchValue)
                        || (x.Adresa != null && x.Adresa.ToLower().Contains(dtParams.SearchValue.ToLower()))
                        || (x.Kat != null && x.Kat.ToLower().Contains(dtParams.SearchValue.ToLower()))
                        || (x.BrojSTana != null && x.BrojSTana.ToLower().Contains(dtParams.SearchValue.ToLower()))
                        || (x.Četvrt != null && x.Četvrt.ToLower().Contains(dtParams.SearchValue.ToLower()))
                        || x.Površina.ToString().Contains(dtParams.SearchValue)
                        || (x.StatusKorištenja != null &&
                        x.StatusKorištenja.ToLower().Contains(dtParams.SearchValue.ToLower()))
                        || (x.Korisnik != null && x.Korisnik.ToLower().Contains(dtParams.SearchValue.ToLower()))
                        || (x.Vlasništvo != null && x.Vlasništvo.ToLower().Contains(dtParams.SearchValue.ToLower())));
        }

        public IEnumerable<Predmet> GetPredmetiForDatatables(IEnumerable<Predmet> predmetList, DTParams Params)
        {
            return predmetList
                    .Where(
                        x => x.Klasa.Contains(Params.SearchValue)
                        || x.Naziv.Contains(Params.SearchValue));
        }

        public IEnumerable<ElektraKupac> GetElektraCustomersForDatatables(IEnumerable<ElektraKupac> ElektraKupacList, DTParams Params )
        {
            return ElektraKupacList
                .Where(
                    x => x.UgovorniRacun.ToString().Contains(Params.SearchValue)
                    || x.Ods.Omm.ToString().Contains(Params.SearchValue)
                    || x.Ods.Stan.StanId.ToString().Contains(Params.SearchValue)
                    || x.Ods.Stan.SifraObjekta.ToString().Contains(Params.SearchValue)
                    || (x.Ods.Stan.Adresa != null && x.Ods.Stan.Adresa.ToLower().Contains(Params.SearchValue.ToLower()))
                    || (x.Ods.Stan.Kat != null && x.Ods.Stan.Kat.ToLower().Contains(Params.SearchValue.ToLower()))
                    || (x.Ods.Stan.BrojSTana != null && x.Ods.Stan.BrojSTana.ToLower().Contains(Params.SearchValue.ToLower()))
                    || (x.Ods.Stan.Četvrt != null && x.Ods.Stan.Četvrt.ToLower().Contains(Params.SearchValue.ToLower()))
                    || x.Ods.Stan.Površina.ToString().Contains(Params.SearchValue)
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(Params.SearchValue.ToLower())));
        }

        public IEnumerable<Dopis> GetLettersForDatatables(IEnumerable<Dopis> DopisList, DTParams Params )
        {
            return DopisList
                .Where(
                    x => x.Predmet.Klasa.Contains(Params.SearchValue)
                    || x.Predmet.Naziv.ToLower().Contains(Params.SearchValue.ToLower())
                    || x.Datum.ToString().Contains(Params.SearchValue)
                    || x.Urbroj.Contains(Params.SearchValue));
        }

        public IEnumerable<ObracunPotrosnje> GetObracunPotrosnjeDatatables(IEnumerable<ObracunPotrosnje> list, DTParams Params)
        {
            return list
                .Where(
                    x => x.BrojBrojila.ToString().Contains(Params.SearchValue)
                    || x.TarifnaStavka.Naziv.Contains(Params.SearchValue)
                    || x.DatumOd.ToString().Contains(Params.SearchValue)
                    || x.DatumDo.ToString().Contains(Params.SearchValue)
                    || x.StanjeOd.ToString().Contains(Params.SearchValue)
                    || x.StanjeDo.ToString().Contains(Params.SearchValue));
        }
    }
}

