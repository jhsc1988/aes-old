using aes.Data;
using aes.Models;
using aes.Repository.IRepository;
using aes.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository
{
    public class StanRepository : Repository<Stan>, IStanRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public StanRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Stan>> GetStanovi()
        {
            // HACK: dummy entity
            return await _unitOfWork.Stan.Find(e => e.Id != 25265);
        }

        public async Task<IEnumerable<Stan>> GetStanoviWithoutODSOmm()
        {
            IEnumerable<Stan> StanList = await _context.Stan
                .FromSqlRaw("select * from Stan where Id not in (select StanId from Ods)")
                .ToListAsync();

            return StanList;
        }
    }
}
