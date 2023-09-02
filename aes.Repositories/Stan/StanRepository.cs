using aes.Data;
using aes.Repository.IRepository;
using aes.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.Stan
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
            return await Context.Stan
                .Where(s => !Context.Ods.Any(o => o.StanId == s.Id))
                .ToListAsync();
        }
    }
}