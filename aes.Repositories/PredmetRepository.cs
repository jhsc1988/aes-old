using aes.Data;
using aes.Models;
using aes.Models.Racuni;
using aes.Repositories.IRepository;

namespace aes.Repositories
{
    public class PredmetRepository : Repository<Predmet>, IPredmetRepository
    {
        public PredmetRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves only Predmeti for paid Racuni, ordered by VrijemeUnosa.
        /// </summary>
        /// <param name="racuni"></param>
        /// <returns></returns>
        public IEnumerable<Predmet> GetPredmetfForAllPayedRacuni(IEnumerable<Racun> racuni)
        {
            return racuni
                .Select(e => e.Dopis.Predmet)
                .OrderByDescending(e => e.VrijemeUnosa)
                .Distinct();
        }

    }
}