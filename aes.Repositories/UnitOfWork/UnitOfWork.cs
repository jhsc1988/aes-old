using aes.Data;
using aes.Repositories.HEP.Elektra;
using aes.Repositories.HEP.ODS;
using aes.Repositories.RacuniRepositories;
using aes.Repositories.RacuniRepositories.Elektra;
using aes.Repositories.Stan;
using aes.Repositories.IRepository;
using aes.Repositories.IRepository.HEP;
using aes.Repositories.RacuniRepositories.IRacuniRepository;
using aes.Repositories.RacuniRepositories.IRacuniRepository.Elektra;

namespace aes.Repositories.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IRacuniElektraRepository RacuniElektra { get; }
    public IRacuniElektraEditRepository RacuniElektraEdit { get; }
    public IRacuniElektraRateRepository RacuniElektraRate { get; }
    public IRacuniElektraRateEditRepository RacuniElektraRateEdit { get; }
    public IRacuniElektraIzvrsenjeUslugeRepository RacuniElektraIzvrsenjeUsluge { get; }
    public IRacuniElektraIzvrsenjeUslugeEditRepository RacuniElektraIzvrsenjeUslugeEdit { get; }
    public IRacuniHoldingRepository RacuniHolding { get; }
    public IRacuniHoldingEditRepository RacuniHoldingEdit { get; }
    public IElektraKupacRepository ElektraKupac { get; }
    public IStanRepository Stan { get; }
    public IOdsRepository Ods { get; }
    public IPredmetRepository Predmet { get; }
    public IDopisRepository Dopis { get; }
    public IStanUpdateRepository StanUpdate { get; }
    public IObracunPotrosnjeRepository ObracunPotrosnje { get; }
    public IOdsEditRepository OdsEdit { get; }
    public IElektraKupacEditRepository ElektraKupacEdit { get; }
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        RacuniElektra = new RacuniElektraRepository(_context);
        RacuniElektraRate = new RacuniElektraRateRepository(_context);
        RacuniElektraIzvrsenjeUsluge = new RacuniElektraIzvrsenjeUslugeRepository(_context);
        RacuniHolding = new RacuniHoldingRepository(_context);
        ElektraKupac = new ElektraKupacRepository(_context);
        Stan = new StanRepository(_context, this);
        Ods = new OdsRepository(_context);
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