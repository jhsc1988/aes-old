using aes.Models;

namespace aes.Repository.IRepository
{
    public interface IStanUpdateRepository : IRepository<StanUpdate>
    {
        StanUpdate GetLatest();
        StanUpdate GetLatestSuccessfulUpdate();
    }
}