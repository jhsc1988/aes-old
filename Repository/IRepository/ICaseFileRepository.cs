using aes.Models;
using aes.Models.Racuni;
using System.Collections.Generic;

namespace aes.Repository.IRepository
{
    public interface ICaseFileRepository : IRepository<Predmet>
    {
        IEnumerable<Predmet> GetCaseFilefForAllPayedBills(IEnumerable<Racun> bills);
    }
}