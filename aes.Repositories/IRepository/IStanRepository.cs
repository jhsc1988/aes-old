namespace aes.Repositories.IRepository;

public interface IStanRepository : IRepository<Models.Stan>
{
    Task<IEnumerable<Models.Stan>> GetStanovi();
    Task<IEnumerable<Models.Stan>> GetStanoviWithoutOdsOmm();
}