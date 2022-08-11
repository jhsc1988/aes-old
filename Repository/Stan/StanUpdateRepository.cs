using aes.Data;
using aes.Models;
using aes.Repository.IRepository;
using System.Linq;

namespace aes.Repository.Stan
{
    public class StanUpdateRepository : Repository<StanUpdate>, IStanUpdateRepository
    {
        public StanUpdateRepository(ApplicationDbContext context) : base(context) { }

        public StanUpdate GetLatest()
        {
            return _context.StanUpdate
                    .OrderByDescending(e => e.UpdateBegan)
                    .FirstOrDefault();
        }

        public StanUpdate GetLatestSuccessfulUpdate()
        {
            return _context.StanUpdate
                .OrderByDescending(e => e.DateOfData)
                .FirstOrDefault();
        }
    }

}
