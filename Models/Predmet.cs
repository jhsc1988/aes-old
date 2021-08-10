using aes.Data;
using System;
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

        public List<Predmet> GetPredmetiDataForFilter(RacunTip tip, ApplicationDbContext _context)
        {

            List<Racun> racunList = new();
            List<Predmet> predmetiList = new();

            switch (tip)
            {
                case RacunTip.RacunElektra:
                    racunList.AddRange(_context.RacunElektra.ToList());
                    break;
                case RacunTip.RacunElektraRate:
                    racunList.AddRange(_context.RacunElektraRate.ToList());
                    break;
                case RacunTip.Holding:
                    racunList.AddRange(_context.RacunHolding.ToList());
                    break;
                case RacunTip.ElektraIzvrsenje:
                    racunList.AddRange(_context.RacunElektraIzvrsenjeUsluge.ToList());
                    break;
                case RacunTip.OdsIzvrsenje:
                    //racunList.AddRange(_context.RacunOdsIzvrsenjaUsluge.ToList());
                    break;
                default:
                    break;
            }

            foreach (Racun e in racunList)
            {
                e.Dopis = _context.Dopis.FirstOrDefault(x => e.DopisId == x.Id);
                if (e.Dopis != null)
                {
                    predmetiList.Add(_context.Predmet.FirstOrDefault(x => e.Dopis.PredmetId == x.Id));
                }
            }
            return predmetiList.Distinct().ToList();
        }

    }
}

