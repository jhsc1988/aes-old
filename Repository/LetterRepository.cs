using aes.Data;
using aes.Models;
using aes.Models.Racuni;
using aes.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository
{
    public class LetterRepository : Repository<Dopis>, ILetterRepository
    {
        public LetterRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Dopis>> GetOnlyEmptyLetters(int predmetId)
        {
            IEnumerable<Dopis> letters = await Find(e => e.PredmetId == predmetId);

            List<Racun> AllBills = new();

            // NOTE: broken open/close principle
            AllBills.AddRange(_context.RacunElektra.Include(e => e.Dopis));
            AllBills.AddRange(_context.RacunElektraRate.Include(e => e.Dopis));
            AllBills.AddRange(_context.RacunElektraIzvrsenjeUsluge.Include(e => e.Dopis));
            AllBills.AddRange(_context.RacunHolding.Include(e => e.Dopis));

            IEnumerable<Dopis> LettersForPayedBills = AllBills.Select(e => e.Dopis).Distinct();

            // returns only empty letters
            return letters.Except(LettersForPayedBills).OrderByDescending(e => e.Datum);
        }

        public async Task<IEnumerable<Dopis>> GetLettersForCaseFile(int predmetId)
        {
            return await _context.Dopis
                .Include(e => e.Predmet)
                .Where(e => e.PredmetId == predmetId)
                .OrderBy(e => e.Datum)
                .ToListAsync();
        }

        public async Task<Dopis> IncludeCaseFile(Dopis letter)
        {
            letter.Predmet = await _context.Predmet.FirstOrDefaultAsync(e => e.Id == letter.PredmetId);
            return letter;
        }
    }
}