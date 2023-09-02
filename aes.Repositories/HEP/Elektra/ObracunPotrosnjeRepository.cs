using aes.Data;
using aes.Models.HEP;
using aes.Repository.IRepository.HEP;
using Microsoft.EntityFrameworkCore;

namespace aes.Repository.HEP.Elektra
{
    public class ObracunPotrosnjeRepository : Repository<ObracunPotrosnje>, IObracunPotrosnjeRepository
    {
        public ObracunPotrosnjeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<ObracunPotrosnje>> GetObracunPotrosnjeForRacun(int racunId)
        {
            return await Context.ObracunPotrosnje.
                Where(e => e.RacunElektraId == racunId)
                .Include(e => e.TarifnaStavka)
                .ToListAsync();
        }

        public async Task<ObracunPotrosnje> GetLastForRacunId(int racunId)
        {
            if (Context.ObracunPotrosnje.Count(e => e.RacunElektraId == racunId) > 1)
            {
                ObracunPotrosnje obracun = await Context.ObracunPotrosnje
                    .Where(e => e.RacunElektraId == racunId)
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
                return await Context.ObracunPotrosnje
                    .Where(e => e.RacunElektraId == racunId)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<ObracunPotrosnje>> GetObracunForUgovorniRacun(long ugovorniRacun)
        {
            return await Context.ObracunPotrosnje
                .Include(e => e.RacunElektra)
                .Include(e => e.RacunElektra.ElektraKupac)
                .Where(e => e.RacunElektra.ElektraKupac.UgovorniRacun == ugovorniRacun)
                .OrderByDescending(e => e.DatumDo)
                .ToListAsync();
        }
    }
}
