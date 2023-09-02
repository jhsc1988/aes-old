using aes.Data;
using aes.Models;
using aes.Repository.IRepository;

namespace aes.Repository.Stan
{
    public class StanUpdateRepository : Repository<StanUpdate>, IStanUpdateRepository
    {
        public StanUpdateRepository(ApplicationDbContext context) : base(context) { }

        /// <summary>
        /// Gets the latest StanUpdate based on when the update began.
        /// </summary>
        /// <returns>The most recently started StanUpdate.</returns>
        public StanUpdate? GetLatestAsync()
        {
            return Context.StanUpdate
                    .OrderByDescending(e => e.UpdateBegan)
                    .FirstOrDefault();
        }

        /// <summary>
        /// Gets the latest 'successful' StanUpdate based on DateOfData.
        /// A successful update here is assumed to be the latest by DateOfData.
        /// </summary>
        /// <returns>The StanUpdate with the most recent DateOfData.</returns>
        public StanUpdate? GetLatestSuccessfulUpdateAsync()
        {
            return Context.StanUpdate
                .OrderByDescending(e => e.DateOfData)
                .FirstOrDefault();
        }
    }

}
