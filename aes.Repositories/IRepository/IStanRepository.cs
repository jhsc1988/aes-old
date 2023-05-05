using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.IRepository
{
    public interface IStanRepository : IRepository<Models.Stan>
    {
        Task<IEnumerable<Models.Stan>> GetStanovi();
        Task<IEnumerable<Models.Stan>> GetStanoviWithoutODSOmm();
    }
}
