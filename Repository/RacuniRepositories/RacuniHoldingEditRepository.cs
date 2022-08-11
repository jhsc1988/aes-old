﻿using aes.Data;
using aes.Models.Racuni.Holding;
using aes.Repository.RacuniRepositories.IRacuniRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository.RacuniRepositories
{
    public class RacuniHoldingEditRepository : Repository<RacunHoldingEdit>, IRacuniHoldingEditRepository
    {
        public RacuniHoldingEditRepository(ApplicationDbContext context) : base(context) { }

        public async Task<RacunHoldingEdit> GetLastRacunHoldingEdit(string userId)
        {
            return await _context.RacunHoldingEdit
                .Include(e => e.RacunHolding)
                .Where(e => e.EditingByUserId == userId)
                .OrderByDescending(e => e.EditTime)
                .FirstOrDefaultAsync();
        }
    }
}
