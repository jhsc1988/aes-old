using aes.Data;
using aes.Models;
using aes.Repository.IRepository;
using System.Linq;

namespace aes.Repository
{
    public class ApartmentUpdateRepository : Repository<ApartmentUpdate>, IApartmentUpdateRepository
    {
        public ApartmentUpdateRepository(ApplicationDbContext context) : base(context) { }

        public ApartmentUpdate getLatest()
        {
            return _context.ApartmentUpdate
                    .OrderByDescending(e => e.UpdateBegan)
                    .FirstOrDefault();
        }

        public ApartmentUpdate getLatestSuccessfulUpdate()
        {
            return _context.ApartmentUpdate
                .OrderByDescending(e => e.DateOfData)
                .FirstOrDefault();
        }
    }

}
