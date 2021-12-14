﻿using aes.Data;
using aes.Models;
using aes.Models.Racuni;
using aes.Repository.BillsRepositories.IBillsRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Repository.BillsRepositories
{
    public class BillsElektraRepository : ElektraRepository<RacunElektra>, IBillsElektraRepository
    {
        public BillsElektraRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<RacunElektra>> GetBillsElektraWithLettersAndCaseFiles()
        {
            return await _context.RacunElektra
                .Include(e => e.Dopis)
                .Include(e => e.Dopis.Predmet)
                .Where(e => e.IsItTemp == null || e.IsItTemp == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Dopis>> GetLettersForPayedBillsElektra(int predmetId)
        {
            // returns only letters for payed bills
            return (await GetBillsElektraWithLettersAndCaseFiles())
                .Where(e => e.Dopis.PredmetId == predmetId)
                .Select(e => e.Dopis)
                .OrderByDescending(e => e.Datum)
                .Distinct();
        }

        public async Task<IEnumerable<RacunElektra>> TempList(string userId)
        {
            return await _context.RacunElektra
                .Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true)
                .Include(r => r.ElektraKupac)
                .Include(r => r.ElektraKupac.Ods)
                .Include(r => r.ElektraKupac.Ods.Stan)
                .Include(r => r.Dopis)
                .Include(r => r.Dopis.Predmet)
                .ToListAsync();
        }

        public async Task<RacunElektra> IncludeAll(int id)
        {
            return await _context.RacunElektra
                .Include(r => r.ElektraKupac)
                .Include(r => r.ElektraKupac.Ods)
                .Include(r => r.ElektraKupac.Ods.Stan)
                .Include(r => r.Dopis)
                .Include(r => r.Dopis.Predmet)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Predmet>> GetCaseFilesForCreate()
        {
            return await _context.Predmet
                .Where(e => e.Archived == false)
                .OrderByDescending(e => e.VrijemeUnosa)
                .ToListAsync();
        }

        public async Task<IEnumerable<RacunElektra>> GetRacuniForCustomer(int kupacId)
        {
            return await _context.RacunElektra
                .Include(e => e.ElektraKupac)
                .Where(e => e.ElektraKupacId == kupacId && (e.IsItTemp == null || false))
                .ToListAsync();
        }
    }
}