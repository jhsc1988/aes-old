using aes.Models;
using aes.Models.HEP;
using aes.Models.Racuni.Elektra;
using aes.Models.Racuni.Holding;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace aes.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<Stan> Stan { get; set; }
        public DbSet<Ods> Ods { get; set; }
        public DbSet<ElektraKupac> ElektraKupac { get; set; }
        public DbSet<Predmet> Predmet { get; set; }
        public DbSet<Dopis> Dopis { get; set; }
        public DbSet<RacunElektra> RacunElektra { get; set; }
        public DbSet<RacunElektraEdit> RacunElektraEdit { get; set; }
        public DbSet<RacunElektraIzvrsenjeUsluge> RacunElektraIzvrsenjeUsluge { get; set; }
        public DbSet<RacunElektraIzvrsenjeUslugeEdit> RacunElektraIzvrsenjeUslugeEdit { get; set; }
        public DbSet<RacunElektraRate> RacunElektraRate { get; set; }
        public DbSet<RacunElektraRateEdit> RacunElektraRateEdit { get; set; }
        public DbSet<RacunHolding> RacunHolding { get; set; }
        public DbSet<RacunHoldingEdit> RacunHoldingEdit { get; set; }
        public DbSet<StanUpdate> StanUpdate { get; set; }
        public DbSet<TarifnaStavka> TarifnaStavka { get; set; }
        public DbSet<ObracunPotrosnje> ObracunPotrosnje { get; set; }
        public DbSet<OdsEdit> OdsEdit { get; set; }
        public DbSet<ElektraKupacEdit> ElektraKupacEdit { get; set; }
    }
}
