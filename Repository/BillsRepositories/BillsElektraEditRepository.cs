using aes.Data;
using aes.Models.Racuni;
using aes.Repository.BillsRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository
{
    public class BillsElektraEditRepository : Repository<RacunElektraEdit>, IBillsElektraEditRepository
    {
        public BillsElektraEditRepository(ApplicationDbContext context) : base(context) { }

        public async Task<RacunElektraEdit> GetLastBillElektraEdit(string userId)
        {
            return await _context.RacunElektraEdit
                .Include(e => e.RacunElektra)
                .Where(e => e.EditingByUserId == userId)
                .OrderByDescending(e => e.EditTime)
                .FirstOrDefaultAsync();
        }
    }
}
