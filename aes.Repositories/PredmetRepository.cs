using aes.Data;
using aes.Models;
using aes.Models.Racuni;
using aes.Repository.IRepository;

namespace aes.Repository
{
    public class PredmetRepository : Repository<Predmet>, IPredmetRepository
    {
        public PredmetRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves only Predmeti for paid Racuni, ordered by VrijemeUnosa.
        /// </summary>
        /// <param name="racuni">The collection of Racun objects to query.</param>
        /// <returns>An ordered, distinct list of Predmet objects.</returns>
        public IEnumerable<Predmet> GetPredmetForAllPaidRacuni(IEnumerable<Racun> racuni)
        {
            return racuni
                .Select(e => e.Dopis.Predmet)
                .OrderByDescending(e => e.VrijemeUnosa)
                .Distinct();
        }

    }
}