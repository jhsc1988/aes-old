namespace aes.Services.BillsServices.IBillsService
{
    public interface IBillService
    {
        int ParseCaseFile(string klasa);
        int ParseLetter(string urbroj);
    }
}