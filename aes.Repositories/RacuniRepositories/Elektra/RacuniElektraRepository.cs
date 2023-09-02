﻿using aes.Data;
using aes.Models;
using aes.Models.Racuni.Elektra;
using aes.Repository.HEP.Elektra;
using aes.Repository.RacuniRepositories.IRacuniRepository.Elektra;
using Microsoft.EntityFrameworkCore;

namespace aes.Repository.RacuniRepositories.Elektra;

public class RacuniElektraRepository : ElektraRepository<RacunElektra>, IRacuniElektraRepository
{
    public RacuniElektraRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<RacunElektra>> GetRacuniElektraWithDopisiAndPredmeti()
    {
        return await Context.RacunElektra
            .Include(e => e.Dopis)
            .Include(e => e.Dopis.Predmet)
            .Where(e => e.IsItTemp == null || e.IsItTemp == false)
            .ToListAsync();
    }

    public async Task<IEnumerable<Dopis>> GetDopisiForPayedRacuniElektra(int predmetId)
    {
        // returns only Dopisi for payed Racuni
        return (await GetRacuniElektraWithDopisiAndPredmeti())
            .Where(e => e.Dopis.PredmetId == predmetId)
            .Select(e => e.Dopis)
            .OrderByDescending(e => e.Datum)
            .Distinct();
    }

    public async Task<IEnumerable<RacunElektra>> TempList(string userId)
    {
        return await Context.RacunElektra
            .Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true)
            .Include(r => r.ElektraKupac)
            .Include(r => r.ElektraKupac.Ods)
            .Include(r => r.ElektraKupac.Ods.Stan)
            .Include(r => r.Dopis)
            .Include(r => r.Dopis.Predmet)
            .OrderBy(r => r.RedniBroj)
            .ToListAsync();
    }

    public async Task<RacunElektra?> IncludeAll(int id)
    {
        return await Context.RacunElektra
            .Include(r => r.ElektraKupac)
            .Include(r => r.ElektraKupac.Ods)
            .Include(r => r.ElektraKupac.Ods.Stan)
            .Include(r => r.Dopis)
            .Include(r => r.Dopis.Predmet)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Predmet>> GetPredmetiForCreate()
    {
        return await Context.Predmet
            .Where(e => e.Archived == false)
            .OrderByDescending(e => e.VrijemeUnosa)
            .ToListAsync();
    }

    public async Task<IEnumerable<RacunElektra>> GetRacuniForCustomer(int kupacId)
    {
        return await Context.RacunElektra
            .Include(e => e.ElektraKupac)
            .Where(e => e.ElektraKupacId == kupacId && (e.IsItTemp == null || false))
            .ToListAsync();
    }
}