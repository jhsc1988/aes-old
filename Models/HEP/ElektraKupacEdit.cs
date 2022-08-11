using System;

namespace aes.Models.HEP
{
    public class ElektraKupacEdit
    {
        public int Id { get; set; }
        public ElektraKupac ElektraKupac { get; set; }
        public string EditingByUserId { get; set; }
        public DateTime EditTime { get; set; }

    }
}
