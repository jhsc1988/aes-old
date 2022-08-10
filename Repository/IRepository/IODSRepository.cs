using aes.Models;
using aes.Models.Racuni;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.IRepository
{
    public interface IODSRepository : IRepository<Ods>
    {
        Task<IEnumerable<Ods>> GetAllOds();
        Task<IEnumerable<TRacun>> GetRacuniForOmm<TRacun>(int stanId) where TRacun : Elektra;
        Task<Ods> IncludeAppartment(Ods ods);
    }
}
