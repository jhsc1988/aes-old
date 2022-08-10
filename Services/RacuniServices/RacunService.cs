using aes.Services.RacuniServices.IRacuniService;

namespace aes.Services.RacuniServices
{
    public class Racuniervice : IRacuniervice
    {
        public int ParsePredmet(string klasa)
        {
            return klasa is null ? 0 : int.Parse(klasa);
        }

        public int ParseDopis(string urbroj)
        {
            return urbroj is null ? 0 : int.Parse(urbroj);
        }
    }
}
