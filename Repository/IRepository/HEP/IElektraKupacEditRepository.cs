using aes.Models.HEP;
using System.Threading.Tasks;

namespace aes.Repository.IRepository.HEP
{
    public interface IElektraKupacEditRepository : IRepository<ElektraKupacEdit>
    {
        Task<ElektraKupacEdit> GetLastElektraKupacEdit(string userId);
    }
}