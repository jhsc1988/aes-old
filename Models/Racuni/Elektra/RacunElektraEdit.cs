using System;

namespace aes.Models.Racuni.Elektra
{
    public class RacunElektraEdit
    {
        public int Id { get; set; }
        public RacunElektra RacunElektra { get; set; }
        public int RacunElektraId { get; set; }
        public string EditingByUserId { get; set; }
        public DateTime EditTime { get; set; }
    }
}
