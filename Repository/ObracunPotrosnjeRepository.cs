using aes.Data;
using aes.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository
{
    public class ObracunPotrosnjeRepository : Repository<ObracunPotrosnje>, IObracunPotrosnjeRepository
    {
        public ObracunPotrosnjeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<ObracunPotrosnje>> GetObracunPotrosnjeForBill(int billId)
        {
            return await _context.ObracunPotrosnje.
                Where(e => e.RacunElektraId == billId)
                .Include(e => e.TarifnaStavka)
                .ToListAsync();
        }

        public async Task<ObracunPotrosnje> GetLastForRacunId(int billId)
        {
            if (_context.ObracunPotrosnje.Where(e => e.RacunElektraId == billId).Count() > 1)
            {
                ObracunPotrosnje obracun = await _context.ObracunPotrosnje
                    .Where(e => e.RacunElektraId == billId)
                    .OrderBy(e => e.Id)
                    .Reverse()
                    .Skip(1) // one before last
                    .FirstOrDefaultAsync();

                obracun.DatumOd = obracun.DatumDo.AddDays(1);
                obracun.DatumDo = obracun.DatumOd.AddMonths(1);
                obracun.StanjeOd = obracun.StanjeDo;
                obracun.StanjeDo = obracun.StanjeOd;

                return obracun;
            }
            else
            {
                return await _context.ObracunPotrosnje
                    .Where(e => e.RacunElektraId == billId)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<ObracunPotrosnje>> GetObracunForUgovorniRacun(long ugovorniRacun)
        {
            return await _context.ObracunPotrosnje
                .Include(e => e.RacunElektra)
                .Include(e => e.RacunElektra.ElektraKupac)
                .Where(e => e.RacunElektra.ElektraKupac.UgovorniRacun == ugovorniRacun)
                .OrderByDescending(e => e.DatumDo)
                .ToListAsync();
        }
    }
}
