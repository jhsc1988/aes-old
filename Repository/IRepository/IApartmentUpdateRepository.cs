using aes.Models;

namespace aes.Repository.IRepository
{
    public interface IApartmentUpdateRepository : IRepository<ApartmentUpdate>
    {
        ApartmentUpdate GetLatest();
        ApartmentUpdate GetLatestSuccessfulUpdate();
    }
}