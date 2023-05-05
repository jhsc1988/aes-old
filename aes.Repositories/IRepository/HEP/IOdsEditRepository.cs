using aes.Models.HEP;
using System.Threading.Tasks;

namespace aes.Repository.IRepository.HEP
{
    public interface IOdsEditRepository : IRepository<OdsEdit>
    {
        Task<OdsEdit> GetLastOdsEdit(string userId);
    }
}