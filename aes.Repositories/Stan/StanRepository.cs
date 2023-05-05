using aes.Data;
using aes.Repositories.IRepository;
using aes.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace aes.Repositories.Stan
{
    public class StanRepository : Repository<Models.Stan>, IStanRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public StanRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Models.Stan>> GetStanovi()
        {
            // HACK: dummy entity
            return await _unitOfWork.Stan.Find(e => e.Id != 25265);
        }

        public async Task<IEnumerable<Models.Stan>> GetStanoviWithoutOdsOmm()
        {
            IEnumerable<Models.Stan> stanList = await Context.Stan
                .FromSqlRaw("select * from Stan where Id not in (select StanId from Ods)")
                .ToListAsync();

            return stanList;
        }
    }
}
