using System;

namespace aes.Models.Racuni.Elektra
{
    public class RacunElektraIzvrsenjeUslugeEdit
    {
        public int Id { get; set; }
        public RacunElektraIzvrsenjeUsluge RacunElektraIzvrsenjeUsluge { get; set; }
        public int RacunElektraIzvrsenjeUslugeId { get; set; }
        public string EditingByUserId { get; set; }
        public DateTime EditTime { get; set; }
    }
}
