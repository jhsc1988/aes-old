using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Threading.Tasks;

namespace aes.Repository.BillsRepositories
{
    public interface IBillsElektraEditRepository : IRepository<RacunElektraEdit>
    {
        Task<RacunElektraEdit> GetLastBillElektraEdit(string userId);
    }
}