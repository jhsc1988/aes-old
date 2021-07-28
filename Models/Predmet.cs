using aes.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace aes.Models
{
    public class Predmet
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(21)]
        public string Klasa { get; set; }

        [MaxLength(60)]
        public string Naziv { get; set; }

        [Display(Name = "Vrijeme unosa")]
        public DateTime? VrijemeUnosa { get; set; } // nullable mi treba za not required

        private readonly ApplicationDbContext _context;

        public Predmet(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Predmet> GetPredmetiDataForFilter(List<Racun> racunList)
        {
            foreach (Racun e in racunList)
            {
                e.Dopis = _context.Dopis.FirstOrDefault(x => e.DopisId == x.Id);
                e.Dopis.Predmet = _context.Predmet.FirstOrDefault(x => e.Dopis.PredmetId == x.Id);
            }
            return racunList.Select(element => element.Dopis.Predmet).Distinct().ToList();
        }

    }
}

