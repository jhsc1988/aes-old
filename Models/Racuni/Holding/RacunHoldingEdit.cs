using System;

namespace aes.Models.Racuni.Holding
{
    public class RacunHoldingEdit
    {
        public int Id { get; set; }
        public RacunHolding RacunHolding { get; set; }
        public int RacunHoldingId { get; set; }
        public string EditingByUserId { get; set; }
        public DateTime EditTime { get; set; }
    }
}
