using aes.Data;
using aes.Models;
using aes.Repositories.IRepository;

namespace aes.Repositories.Stan
{
    public class StanUpdateRepository : Repository<StanUpdate>, IStanUpdateRepository
    {
        public StanUpdateRepository(ApplicationDbContext context) : base(context) { }

        public StanUpdate? GetLatest()
        {
            return Context.StanUpdate
                    .OrderByDescending(e => e.UpdateBegan)
                    .FirstOrDefault();
        }

        public StanUpdate? GetLatestSuccessfulUpdate()
        {
            return Context.StanUpdate
                .OrderByDescending(e => e.DateOfData)
                .FirstOrDefault();
        }
    }

}
