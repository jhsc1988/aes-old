using aes.Models.HEP;
using aes.Models.Racuni.Elektra;

namespace aes.Repository.IRepository.HEP
{
    public interface IElektraRepository<T> : IRepository<T> where T : Elektra
    {
        Task<ElektraKupac> GetKupacByUgovorniRacun(long uRacun);
        Task<IEnumerable<T>> GetRacuni(int predmetId, int dopisId);
        Task<IEnumerable<T>> GetRacuniTemp(string userId);
    }
}
