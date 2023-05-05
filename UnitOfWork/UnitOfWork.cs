using aes.Data;
using aes.Repository.HEP.Elektra;
using aes.Repository.HEP.ODS;
using aes.Repository.IRepository;
using aes.Repository.IRepository.HEP;
using aes.Repository.RacuniRepositories;
using aes.Repository.RacuniRepositories.Elektra;
using aes.Repository.RacuniRepositories.IRacuniRepository;
using aes.Repository.RacuniRepositories.IRacuniRepository.Elektra;
using aes.Repository.Stan;
using System;
using System.Threading.Tasks;

namespace aes.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IRacuniElektraRepository RacuniElektra { get; private set; }
        public IRacuniElektraEditRepository RacuniElektraEdit { get; private set; }
        public IRacuniElektraRateRepository RacuniElektraRate { get; private set; }
        public IRacuniElektraRateEditRepository RacuniElektraRateEdit { get; private set; }
        public IRacuniElektraIzvrsenjeUslugeRepository RacuniElektraIzvrsenjeUsluge { get; private set; }
        public IRacuniElektraIzvrsenjeUslugeEditRepository RacuniElektraIzvrsenjeUslugeEdit { get; private set; }
        public IRacuniHoldingRepository RacuniHolding { get; private set; }
        public IRacuniHoldingEditRepository RacuniHoldingEdit { get; private set; }
        public IElektraKupacRepository ElektraKupac { get; private set; }
        public IStanRepository Stan { get; private set; }
        public IODSRepository Ods { get; private set; }
        public IPredmetRepository Predmet { get; private set; }
        public IDopisRepository Dopis { get; private set; }
        public IStanUpdateRepository StanUpdate { get; private set; }
        public IObracunPotrosnjeRepository ObracunPotrosnje { get; private set; }
        public IOdsEditRepository OdsEdit { get; private set; }
        public IElektraKupacEditRepository ElektraKupacEdit { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {

            _context = context;
            RacuniElektra = new RacuniElektraRepository(_context);
            RacuniElektraRate = new RacuniElektraRateRepository(_context);
            RacuniElektraIzvrsenjeUsluge = new RacuniElektraIzvrsenjeUslugeRepository(_context);
            RacuniHolding = new RacuniHoldingRepository(_context);
            ElektraKupac = new ElektraKupacRepository(_context);
            Stan = new StanRepository(_context, this);
            Ods = new ODSRepository(_context);
            Predmet = new PredmetRepository(_context);
            Dopis = new DopisRepository(_context);
            StanUpdate = new StanUpdateRepository(_context);
            RacuniHoldingEdit = new RacuniHoldingEditRepository(_context);
            RacuniElektraEdit = new RacuniElektraEditRepository(_context);
            RacuniElektraRateEdit = new RacuniElektraRateEditRepository(_context);
            RacuniElektraIzvrsenjeUslugeEdit = new RacuniElektraIzvrsenjeUslugeEditRepository(_context);
            ObracunPotrosnje = new ObracunPotrosnjeRepository(_context);
            OdsEdit = new OdsEditRepository(_context);
            ElektraKupacEdit = new ElektraKupacEditRepository(_context);
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
