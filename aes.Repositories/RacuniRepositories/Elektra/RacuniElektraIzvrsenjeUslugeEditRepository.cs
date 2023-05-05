using aes.Data;
using aes.Models.Racuni.Elektra;
using aes.Repository.RacuniRepositories.IRacuniRepository.Elektra;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository.RacuniRepositories.Elektra
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
