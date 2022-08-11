using aes.Models.HEP;

namespace aes.Models.Racuni.Elektra
{
    public abstract class Elektra : Racun
    {
        public ElektraKupac ElektraKupac { get; set; }
        public int? ElektraKupacId { get; set; }
    }
}
