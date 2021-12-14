using System;

namespace aes.Services.BillsServices.IBillsService
{
    public interface IBillsValidationService
    {
        bool Validate(string brojRacuna, string iznos, string date, string dopisId, out string msg, out double _iznos, out int _dopisId, out DateTime? datumIzdavanja);
    }
}