using aes.Models;
using aes.Models.Racuni;

namespace aes.Repository.IRepository
{
    public interface IPredmetRepository : IRepository<Predmet>
    {
        IEnumerable<Predmet> GetPredmetForAllPaidRacuni(IEnumerable<Racun> racuni);
    }
}