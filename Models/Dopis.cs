﻿using aes.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace aes.Models
{
    public class Dopis
    {
        public int Id { get; set; }

        public Predmet Predmet { get; set; }
        public int PredmetId { get; set; }

        [Required]
        [MaxLength(25)]
        public string Urbroj { get; set; }

        public DateTime? Datum { get; set; }

        [Display(Name = "Vrijeme unosa")]
        public DateTime? VrijemeUnosa { get; set; } // nullable mi treba za not required

        private readonly ApplicationDbContext _context;

        public Dopis(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Dopis> GetDopisiDataForFilter(int predmetId)
        {
            List<Dopis> dopisList = _context.Dopis.ToList();
            return dopisList.Where(element => element.PredmetId == predmetId).ToList();
        }

    }
}
