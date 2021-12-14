using aes.Services.BillsServices.IBillsService;

namespace aes.Services.BillsServices
{
    public class BillService : IBillService
    {
        public int ParseCaseFile(string klasa)
        {
            return klasa is null ? 0 : int.Parse(klasa);
        }

        public int ParseLetter(string urbroj)
        {
            return urbroj is null ? 0 : int.Parse(urbroj);
        }
    }
}
