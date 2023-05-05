using aes.Models.HEP;

namespace aes.Repositories.IRepository.HEP
{
    public interface IObracunPotrosnjeRepository : IRepository<ObracunPotrosnje>
    {
        Task<ObracunPotrosnje?> GetLastForRacunId(int racunId);
        Task<IEnumerable<ObracunPotrosnje>> GetObracunForUgovorniRacun(long ugovorniRacun);
        Task<IEnumerable<ObracunPotrosnje>> GetObracunPotrosnjeForRacun(int racunId);
    }
}