using aes.Data;
using aes.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository
{
    public class OdsEditRepository : Repository<OdsEdit>, IOdsEditRepository
    {
        public OdsEditRepository(ApplicationDbContext context) : base(context) { }
        public async Task<OdsEdit> GetLastOdsEdit(string userId)
        {
            return await _context.OdsEdit
                .Include(e => e.Ods)
                .Where(e => e.EditingByUserId == userId)
                .OrderByDescending(e => e.EditTime)
                .FirstOrDefaultAsync();
        }
    }
}
