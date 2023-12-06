namespace aes.Repositories.IRepository;

namespace aes.Repository.IRepository
{
    public interface IStanRepository : IRepository<Models.Stan>
    {
        ApplicationDbContext Context { get; }
        Task<IEnumerable<Models.Stan>> GetStanovi();
        Task<IEnumerable<Models.Stan>> GetStanoviWithoutOdsOmm();
    }
}