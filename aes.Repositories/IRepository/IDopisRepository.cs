using aes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.IRepository
{
    public interface IDopisRepository : IRepository<Dopis>
    {
        Task<IEnumerable<Dopis>> GetDopisiForPredmet(int predmetId);
        Task<IEnumerable<Dopis>> GetOnlyEmptyDopisi(int predmetId);
        Task<Dopis> IncludePredmet(Dopis Dopis);
    }
}