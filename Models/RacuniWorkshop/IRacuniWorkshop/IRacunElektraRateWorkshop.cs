﻿using aes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public interface IRacunElektraRateWorkshop
    {
        JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId, string userId, ApplicationDbContext _context);
        List<RacunElektraRate> GetListCreateList(string userId, ApplicationDbContext _context);
        List<RacunElektraRate> GetList(int predmetIdAsInt, int dopisIdAsInt, ApplicationDbContext _context);
        List<RacunElektraRate> GetRacuniElektraRateForDatatables(IDatatablesParams Params, ApplicationDbContext _context, List<RacunElektraRate> CreateRacuniElektraRateList);
        JsonResult GetList(bool isFiltered, string klasa, string urbroj, IDatatablesGenerator datatablesGenerator, ApplicationDbContext _context, HttpRequest Request, string Uid);
    }
}
