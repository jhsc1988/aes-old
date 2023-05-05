using aes.Repositories.IRepository;
using aes.Repositories.IRepository.HEP;
using aes.Repositories.RacuniRepositories.IRacuniRepository;
using aes.Repositories.RacuniRepositories.IRacuniRepository.Elektra;

namespace aes.Repositories.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IRacuniElektraRepository RacuniElektra { get; }
    IRacuniElektraRateRepository RacuniElektraRate { get; }
    IRacuniElektraIzvrsenjeUslugeRepository RacuniElektraIzvrsenjeUsluge { get; }
    IStanRepository Stan { get; }
    IOdsRepository Ods { get; }
    IPredmetRepository Predmet { get; }
    IRacuniHoldingRepository RacuniHolding { get; }
    IDopisRepository Dopis { get; }
    IElektraKupacRepository ElektraKupac { get; }
    IStanUpdateRepository StanUpdate { get; }
    IRacuniHoldingEditRepository RacuniHoldingEdit { get; }
    IRacuniElektraEditRepository RacuniElektraEdit { get; }
    IRacuniElektraRateEditRepository RacuniElektraRateEdit { get; }
    IRacuniElektraIzvrsenjeUslugeEditRepository RacuniElektraIzvrsenjeUslugeEdit { get; }
    IObracunPotrosnjeRepository ObracunPotrosnje { get; }
    IOdsEditRepository OdsEdit { get; }
    IElektraKupacEditRepository ElektraKupacEdit { get; }

    Task<int> Complete();
}