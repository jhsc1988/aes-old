using aes.Data;
using aes.Models.HEP;
using aes.Repositories.IRepository;
using aes.Repositories.IRepository.HEP;
using Microsoft.EntityFrameworkCore;

namespace aes.Repositories.HEP.ODS
{
    public class OdsEditRepository : Repository<OdsEdit>, IOdsEditRepository
    {
        public OdsEditRepository(ApplicationDbContext context) : base(context) { }
        public async Task<OdsEdit?> GetLastOdsEdit(string userId) =>
            await Context.OdsEdit
                .Include(e => e.Ods)
                .Where(e => e.EditingByUserId == userId)
                .OrderByDescending(e => e.EditTime)
                .FirstOrDefaultAsync();
    }
}
