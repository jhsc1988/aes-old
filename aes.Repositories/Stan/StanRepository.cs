using aes.Data;
using aes.Repositories.IRepository;
using aes.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace aes.Repositories.Stan
{
    public class StanRepository : Repository<Models.Stan>, IStanRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public new ApplicationDbContext Context { get; }

        private static readonly Func<ApplicationDbContext, int, IEnumerable<Models.Stan>> CompiledQuery 
            = EF.CompileQuery((ApplicationDbContext dbContext, int id) => dbContext.Stan.Where(e => e.Id != id));

        public StanRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
        {
            Context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Models.Stan>> GetStanovi()
        {
            // HACK: dummy entity
            // return await _unitOfWork.Stan.Find(e => e.Id != 25265);
            var dbContext = _unitOfWork.Stan.Context;
            var result = CompiledQuery(dbContext, 25265).ToList();
            return await Task.FromResult(result);
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