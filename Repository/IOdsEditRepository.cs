using aes.Models;
using aes.Repository.IRepository;
using System.Threading.Tasks;

namespace aes.Repository
{
    public interface IOdsEditRepository : IRepository<OdsEdit>
    {
        Task<OdsEdit> GetLastOdsEdit(string userId);
    }
}