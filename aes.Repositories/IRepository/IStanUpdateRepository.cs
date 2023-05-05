using aes.Models;

namespace aes.Repositories.IRepository
{
    public interface IStanUpdateRepository : IRepository<StanUpdate>
    {
        StanUpdate? GetLatest();
        StanUpdate? GetLatestSuccessfulUpdate();
    }
}