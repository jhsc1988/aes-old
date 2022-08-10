using aes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.IRepository
{
    public interface IStanRepository : IRepository<Stan>
    {
        Task<IEnumerable<Stan>> GetStanovi();
        Task<IEnumerable<Stan>> GetStanoviWithoutODSOmm();
    }
}
