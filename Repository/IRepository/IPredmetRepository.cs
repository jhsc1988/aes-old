using aes.Models;
using aes.Models.Racuni;
using System.Collections.Generic;

namespace aes.Repository.IRepository
{
    public interface IPredmetRepository : IRepository<Predmet>
    {
        IEnumerable<Predmet> GetPredmetfForAllPayedRacuni(IEnumerable<Racun> Racuni);
    }
}