using aes.Data;
using aes.Models.Racuni.Elektra;
using aes.Repositories.IRepository;
using aes.Repositories.RacuniRepositories.IRacuniRepository.Elektra;
using Microsoft.EntityFrameworkCore;

namespace aes.Repositories.RacuniRepositories.Elektra
{
public class RacuniElektraIzvrsenjeUslugeEditRepository : Repository<RacunElektraIzvrsenjeUslugeEdit>, IRacuniElektraIzvrsenjeUslugeEditRepository
{
    public RacuniElektraIzvrsenjeUslugeEditRepository(ApplicationDbContext context) : base(context) { }

        public async Task<RacunElektraIzvrsenjeUslugeEdit?> GetLastRacunElektraServiceEdit(string userId) =>
            await Context.RacunElektraIzvrsenjeUslugeEdit
            .Include(e => e.RacunElektraIzvrsenjeUsluge)
            .Where(e => e.EditingByUserId == userId)
            .OrderByDescending(e => e.EditTime)
            .FirstOrDefaultAsync();
    }

}