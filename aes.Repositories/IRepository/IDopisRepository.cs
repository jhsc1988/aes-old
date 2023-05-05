using aes.Models;

namespace aes.Repositories.IRepository
{
    public interface IDopisRepository : IRepository<Dopis>
    {
        Task<IEnumerable<Dopis>> GetDopisiForPredmet(int predmetId);
        Task<IEnumerable<Dopis>> GetOnlyEmptyDopisi(int predmetId);
        Task<Dopis> IncludePredmet(Dopis dopis);
    }
}