using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace aes.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<aes.Models.Stan> Stan { get; set; }
        public DbSet<aes.Models.Ods> Ods { get; set; }
        public DbSet<aes.Models.ElektraKupac> ElektraKupac { get; set; }
        public DbSet<aes.Models.Predmet> Predmet { get; set; }
        public DbSet<aes.Models.Dopis> Dopis { get; set; }
        public DbSet<aes.Models.RacunElektra> RacunElektra { get; set; }
        public DbSet<aes.Models.RacunElektraIzvrsenjeUsluge> RacunElektraIzvrsenjeUsluge { get; set; }
        public DbSet<aes.Models.RacunElektraRate> RacunElektraRate { get; set; }
        public DbSet<aes.Models.RacunHolding> RacunHolding { get; set; }

    }
}
