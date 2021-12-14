using aes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.IRepository
{
    public interface IElektraCustomerRepository : IRepository<ElektraKupac>
    {
        Task<ElektraKupac> FindExact(long ugovorniRacun);
        Task<ElektraKupac> FindExactById(int id);
        Task<IEnumerable<ElektraKupac>> GetAllCustomers();
        Task<ElektraKupac> IncludeOdsAndApartment(ElektraKupac elektraKupac);
    }
}