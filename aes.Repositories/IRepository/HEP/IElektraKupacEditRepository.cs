using aes.Models.HEP;

namespace aes.Repositories.IRepository.HEP
{
    public interface IElektraKupacEditRepository : IRepository<ElektraKupacEdit>
    {
        Task<ElektraKupacEdit?> GetLastElektraKupacEdit(string userId);
    }
}