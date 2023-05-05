using aes.Models;
using aes.Models.Racuni;

namespace aes.Repositories.IRepository
{
    public interface IPredmetRepository : IRepository<Predmet>
    {
        IEnumerable<Predmet> GetPredmetfForAllPayedRacuni(IEnumerable<Racun> racuni);
    }
}