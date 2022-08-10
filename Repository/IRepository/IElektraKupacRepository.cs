using aes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.IRepository
{
    public interface IElektraKupacRepository : IRepository<ElektraKupac>
    {
        Task<ElektraKupac> FindExact(long ugovorniRacun);
        Task<ElektraKupac> FindExactById(int id);
        Task<IEnumerable<ElektraKupac>> GetAllCustomers();
        Task<ElektraKupac> IncludeOdsAndStan(ElektraKupac elektraKupac);
    }
}