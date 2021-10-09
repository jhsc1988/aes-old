using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace aes.Models
{
    public class ElektraKupacWorkshop : IElektraKupacWorkshop
    {
        public List<ElektraKupac> GetKupciForDatatables(DatatablesParams Params, List<ElektraKupac> ElektraKupacList)
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
                    || (x.Napomena != null && x.Napomena.ToLower().Contains(Params.SearchValue.ToLower()))).ToDynamicList<ElektraKupac>();
        }

        public List<RacunElektraIzvrsenjeUsluge> GetRacuniElektraIzvrsenjeForKupac(int param)
        {
            throw new NotImplementedException();
        }

        public List<RacunElektra> GetRacuniForKupac(int param)
        {
            throw new NotImplementedException();
        }

        public List<RacunElektraRate> GetRacuniRateForKupac(int param)
        {
            throw new NotImplementedException();
        }
    }
}
