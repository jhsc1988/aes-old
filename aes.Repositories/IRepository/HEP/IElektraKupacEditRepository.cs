using aes.Models.HEP;

namespace aes.Repository.IRepository.HEP
{
    public interface IElektraKupacEditRepository : IRepository<ElektraKupacEdit>
    {
        Task<ElektraKupacEdit> GetLastElektraKupacEdit(string userId);
    }
}