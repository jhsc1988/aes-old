using aes.Repository.BillsRepositories;
using aes.Repository.BillsRepositories.IBillsRepository;
using aes.Repository.IRepository;
using System;
using System.Threading.Tasks;

namespace aes.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBillsElektraRepository BillsElektra { get; }
        IBillsElektraAdvancesRepository BillsElektraAdvances { get; }
        IBillsElektraServicesRepository BillsElektraServices { get; }
        IApartmentRepository Apartment { get; }
        IODSRepository Ods { get; }
        ICaseFileRepository CaseFile { get; }
        IBillsHoldingRepository BillsHolding { get; }
        ILetterRepository Letter { get; }
        IElektraCustomerRepository ElektraCustomer { get; }
        IApartmentUpdateRepository ApartmentUpdate { get; }
        IBillsHoldingEditRepository BillsHoldingEdit { get; }
        IBillsElektraEditRepository BillsElektraEdit { get; }
        IBillsElektraAdvancesEditRepository BillsElektraAdvancesEdit { get; }
        IBillsElektraServicesEditRepository BillsElektraServicesEdit { get; }
        IObracunPotrosnjeRepository ObracunPotrosnje { get; }
        IOdsEditRepository OdsEdit { get; }
        IElektraCustomerEditRepository ElektraCustomerEdit { get; }

        Task<int> Complete();
    }
}
