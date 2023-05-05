using aes.Data;
using aes.Models;
using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace aes.Repository
{
    public class PredmetRepository : Repository<Predmet>, IPredmetRepository
    {
        public PredmetRepository(ApplicationDbContext context) : base(context) { }

        /// <summary>
        /// only Predmeti for payed Racuni ordered by VrijemeUnosa
        /// </summary>
        /// <param name="Racuni"></param>
        /// <returns></returns>
        public IEnumerable<Predmet> GetPredmetfForAllPayedRacuni(IEnumerable<Racun> Racuni)
        {
            return Racuni
                .Select(e => e.Dopis.Predmet)
                .OrderByDescending(e => e.VrijemeUnosa)
                .Distinct();
        }
    }
}
