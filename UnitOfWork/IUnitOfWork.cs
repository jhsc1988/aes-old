using aes.Data;
using aes.Repository.IRepository;
using aes.Repository.IRepository.HEP;
using aes.Repository.RacuniRepositories.IRacuniRepository;
using aes.Repository.RacuniRepositories.IRacuniRepository.Elektra;
using Microsoft.EntityFrameworkCore;

namespace aes.UnitOfWork
{
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
}
