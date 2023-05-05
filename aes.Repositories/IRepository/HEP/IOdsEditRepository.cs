using aes.Models.HEP;

namespace aes.Repositories.IRepository.HEP
{
    public interface IOdsEditRepository : IRepository<OdsEdit>
    {
        Task<OdsEdit?> GetLastOdsEdit(string userId);
    }
}