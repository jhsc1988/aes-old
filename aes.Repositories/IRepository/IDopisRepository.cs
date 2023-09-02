using aes.Models;

namespace aes.Repository.IRepository
{
    public interface IDopisRepository : IRepository<Dopis>
    {
        Task<IEnumerable<Dopis>> GetDopisiForPredmet(int predmetId);
        Task<IEnumerable<Dopis>> GetOnlyEmptyDopisiAsync(int predmetId);
        Task<Dopis> IncludePredmetAsync(Dopis dopis);
    }
}