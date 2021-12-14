using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Threading.Tasks;

namespace aes.Repository.BillsRepositories
{
    public interface IBillsElektraAdvancesEditRepository : IRepository<RacunElektraRateEdit>
    {
        Task<RacunElektraRateEdit> GetLastBillElektraAdvancesEdit(string userId);
    }
}