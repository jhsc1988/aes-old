using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Threading.Tasks;

namespace aes.Repository.BillsRepositories
{
    public interface IBillsElektraServicesEditRepository : IRepository<RacunElektraIzvrsenjeUslugeEdit>
    {
        Task<RacunElektraIzvrsenjeUslugeEdit> GetLastBillElektraServiceEdit(string userId);
    }
}