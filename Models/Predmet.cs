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
            List<Predmet> lp = new();
            foreach (Racun e in racunList)
            {
                e.Dopis = _context.Dopis.FirstOrDefault(x => e.DopisId == x.Id);
                if (e.Dopis != null)
                {
                    lp.Add(_context.Predmet.FirstOrDefault(x => e.Dopis.PredmetId == x.Id));
                }
            }
            var v = lp.Distinct();

            return v.ToList();
        }

    }
}

