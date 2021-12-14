using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Threading.Tasks;

namespace aes.Repository.BillsRepositories.IBillsRepository
{
    public interface IBillsHoldingEditRepository : IRepository<RacunHoldingEdit>
    {
        Task<RacunHoldingEdit> GetLastBillHoldingEdit(string userId);
    }
}