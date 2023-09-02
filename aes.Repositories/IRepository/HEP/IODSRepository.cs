using aes.Models.HEP;
using aes.Models.Racuni.Elektra;

namespace aes.Repository.IRepository.HEP
{
    public interface IOdsRepository : IRepository<Ods>
    {
        Task<IEnumerable<Ods>> GetAllOds();
        Task<IEnumerable<TRacun>> GetRacuniForOmm<TRacun>(int stanId) where TRacun : Elektra;
        Task<Ods> IncludeApartment(Ods ods);
    }
}
