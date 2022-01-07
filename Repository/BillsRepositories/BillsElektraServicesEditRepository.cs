using aes.Data;
using aes.Models.Racuni;
using aes.Repository.BillsRepositories.IBillsRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository.BillsRepositories
{
    public class BillsElektraServicesEditRepository : Repository<RacunElektraIzvrsenjeUslugeEdit>, IBillsElektraServicesEditRepository
    {
        public BillsElektraServicesEditRepository(ApplicationDbContext context) : base(context) { }

        public async Task<RacunElektraIzvrsenjeUslugeEdit> GetLastBillElektraServiceEdit(string userId)
        {
            return await _context.RacunElektraIzvrsenjeUslugeEdit
                .Include(e => e.RacunElektraIzvrsenjeUsluge)
                .Where(e => e.EditingByUserId == userId)
                .OrderByDescending(e => e.EditTime)
                .FirstOrDefaultAsync();
        }

    }
}
