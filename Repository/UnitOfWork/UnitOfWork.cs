using aes.Data;
using aes.Repository.BillsRepositories;
using aes.Repository.BillsRepositories.IBillsRepository;
using aes.Repository.IRepository;
using System;
using System.Threading.Tasks;

namespace aes.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IBillsElektraRepository BillsElektra { get; private set; }
        public IBillsElektraEditRepository BillsElektraEdit { get; private set; }
        public IBillsElektraAdvancesRepository BillsElektraAdvances { get; private set; }
        public IBillsElektraAdvancesEditRepository BillsElektraAdvancesEdit { get; private set; }
        public IBillsElektraServicesRepository BillsElektraServices { get; private set; }
        public IBillsElektraServicesEditRepository BillsElektraServicesEdit { get; private set; }
        public IBillsHoldingRepository BillsHolding { get; private set; }
        public IBillsHoldingEditRepository BillsHoldingEdit { get; private set; }
        public IElektraCustomerRepository ElektraCustomer { get; private set; }
        public IApartmentRepository Apartment { get; private set; }
        public IODSRepository Ods { get; private set; }
        public ICaseFileRepository CaseFile { get; private set; }
        public ILetterRepository Letter { get; private set; }
        public IApartmentUpdateRepository ApartmentUpdate { get; private set; }
        public IObracunPotrosnjeRepository ObracunPotrosnje { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {

            _context = context;
            BillsElektra = new BillsElektraRepository(_context);
            BillsElektraAdvances = new BillsElektraAdvancesRepository(_context);
            BillsElektraServices = new BillsElektraServicesRepository(_context);
            BillsHolding = new BillsHoldingRepository(_context);
            ElektraCustomer = new ElektraCustomerRepository(_context);
            Apartment = new ApartmentRepository(_context, this);
            Ods = new ODSRepository(_context);
            CaseFile = new CaseFileRepository(_context);
            Letter = new LetterRepository(_context);
            ApartmentUpdate = new ApartmentUpdateRepository(_context);
            BillsHoldingEdit = new BillsHoldingEditRepository(_context);
            BillsElektraEdit = new BillsElektraEditRepository(_context);
            BillsElektraAdvancesEdit = new BillsElektraAdvancesEditRepository(_context);
            BillsElektraServicesEdit = new BillsElektraServicesEditRepository(_context);
            ObracunPotrosnje = new ObracunPotrosnjeRepository(_context);
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
