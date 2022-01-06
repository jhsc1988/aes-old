using aes.Models;
using aes.Repository.IRepository;
using System.Threading.Tasks;

namespace aes.Repository
{
    public interface IElektraCustomerEditRepository : IRepository<ElektraKupacEdit>
    {
        Task<ElektraKupacEdit> GetLastElektraKupacEdit(string userId);
    }
}