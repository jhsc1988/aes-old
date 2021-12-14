using aes.Models;
using aes.Models.Racuni;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.IRepository
{
    public interface IElektraRepository<T> : IRepository<T> where T : Elektra
    {
        Task<ElektraKupac> GetKupacByUgovorniRacun(long URacun);
        Task<IEnumerable<T>> GetRacuni(int predmetId, int dopisId);
        Task<IEnumerable<T>> GetRacuniTemp(string userId);
    }
}
