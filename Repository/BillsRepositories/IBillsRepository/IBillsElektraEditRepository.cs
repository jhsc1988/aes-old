using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Threading.Tasks;

namespace aes.Repository.BillsRepositories.IBillsRepository
{
    public interface IBillsElektraEditRepository : IRepository<RacunElektraEdit>
    {
        Task<RacunElektraEdit> GetLastBillElektraEdit(string userId);
    }
}