using aes.Data;
using aes.Models.Racuni;
using aes.Repository.RacuniRepositories.IRacuniRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository.RacuniRepositories
{
    public class RacuniElektraIzvrsenjeUslugeEditRepository : Repository<RacunElektraIzvrsenjeUslugeEdit>, IRacuniElektraIzvrsenjeUslugeEditRepository
    {
        public RacuniElektraIzvrsenjeUslugeEditRepository(ApplicationDbContext context) : base(context) { }

        public async Task<RacunElektraIzvrsenjeUslugeEdit> GetLastRacunElektraServiceEdit(string userId)
        {
            return await _context.RacunElektraIzvrsenjeUslugeEdit
                .Include(e => e.RacunElektraIzvrsenjeUsluge)
                .Where(e => e.EditingByUserId == userId)
                .OrderByDescending(e => e.EditTime)
                .FirstOrDefaultAsync();
        }

    }
}
