using aes.Models.HEP;

namespace aes.Repository.IRepository.HEP
{
    public interface IOdsEditRepository : IRepository<OdsEdit>
    {
        Task<OdsEdit?> GetLastOdsEdit(string userId);
    }
}