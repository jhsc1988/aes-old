using aes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.IRepository
{
    public interface IApartmentRepository : IRepository<Stan>
    {
        Task<IEnumerable<Stan>> GetApartments();
        Task<IEnumerable<Stan>> GetApartmentsWithoutODSOmm();
    }
}
