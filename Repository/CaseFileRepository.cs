using aes.Data;
using aes.Models;
using aes.Models.Racuni;
using aes.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace aes.Repository
{
    public class CaseFileRepository : Repository<Predmet>, ICaseFileRepository
    {
        public CaseFileRepository(ApplicationDbContext context) : base(context) { }

        /// <summary>
        /// only casefiles for payed bills ordered by VrijemeUnosa
        /// </summary>
        /// <param name="bills"></param>
        /// <returns></returns>
        public IEnumerable<Predmet> GetCaseFilefForAllPayedBills(IEnumerable<Racun> bills)
        {
            return bills
                .Select(e => e.Dopis.Predmet)
                .OrderByDescending(e => e.VrijemeUnosa)
                .Distinct();
        }
    }
}
