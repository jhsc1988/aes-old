using aes.Data;
using aes.Models.HEP;
using aes.Repository.IRepository.HEP;
using Microsoft.EntityFrameworkCore;

namespace aes.Repository.HEP.ODS
{
    public class OdsEditRepository : Repository<OdsEdit>, IOdsEditRepository
    {
        public OdsEditRepository(ApplicationDbContext context) : base(context) { }
        public async Task<OdsEdit?> GetLastOdsEdit(string userId)
        {
            return await Context.OdsEdit
                .Include(e => e.Ods)
                .Where(e => e.EditingByUserId == userId)
                .OrderByDescending(e => e.EditTime)
                .FirstOrDefaultAsync();
        }
    }
}
