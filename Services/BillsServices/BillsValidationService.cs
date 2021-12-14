using aes.Services.BillsServices.IBillsService;
using System;
using System.Globalization;

namespace aes.Services.BillsServices
{
    public class BillsValidationService : IBillsValidationService
    {
        // todo: ovaj validate mi vjerojatno ni ne treba
        public bool Validate(string brojRacuna, string iznos, string date, string dopisId, out string msg, out double _iznos, out int _dopisId, out DateTime? datumIzdavanja)
        {
            _iznos = 0;
            datumIzdavanja = null;
            msg = null;
            DateTime dt;

            if (!int.TryParse(dopisId, out _dopisId))
            {
                msg = "Dopis ID je neispravan";
                return false;
            }

            if (brojRacuna == null)
            {
                msg = "Broj računa je obavezan";
                return false;
            }

            if (date == null)
            {
                msg = "Datum izdavanja je obavezan";
                return false;
            }

            else if (!DateTime.TryParse(date, out dt)) { msg = "Datum izdavanja je obavezan"; return false; }

            // "en-US" mi treba za decimalnu tocku, decimanlna točka mi treba za Excel export
            if (!double.TryParse(iznos, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out _iznos))
            {
                msg = "Iznos je neispravan";
                return false;
            }

            if (double.Parse(iznos) <= 0)
            {
                msg = "Iznos mora biti veći od 0 kn";
                return false;
            }

            datumIzdavanja = dt;
            return true;
        }
    }
}
