using aes.Models.Racuni.Holding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.RacuniHoldingService.IService
{
    public interface IRacuniHoldingService : IRacuniService.IRacuniervice
    {
        Task<IEnumerable<RacunHolding>> GetCreateRacuni(string userId);
        Task<IEnumerable<RacunHolding>> GetList(int predmetIdAsInt, int dopisIdAsInt);
    }
}