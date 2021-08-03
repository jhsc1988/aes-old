using aes.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace aes.Models
{
    public class RacunElektraIzvrsenjeUsluge : Racun
    {

        public ElektraKupac ElektraKupac { get; set; }
        [Required]
        public int ElektraKupacId { get; set; }

        // required se podrazumijeva jer nije nullable
        [Display(Name = "Datum izvršenja")]
        public DateTime DatumIzvrsenja { get; set; }

        public static List<RacunElektraIzvrsenjeUsluge> GetList(int predmetIdAsInt, int dopisIdAsInt, ApplicationDbContext _context)
        {
            List<RacunElektraIzvrsenjeUsluge> racunElektraIzvrsenjeList = new();

            if (predmetIdAsInt == 0 && dopisIdAsInt == 0)
            {
                racunElektraIzvrsenjeList = _context.RacunElektraIzvrsenjeUsluge.Where(e => e.IsItTemp == null).ToList();
            }

            if (predmetIdAsInt != 0)
            {
                racunElektraIzvrsenjeList = dopisIdAsInt == 0
                    ? _context.RacunElektraIzvrsenjeUsluge.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt).ToList()
                    : _context.RacunElektraIzvrsenjeUsluge.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt && x.Dopis.Id == dopisIdAsInt).ToList();
            }


            foreach (RacunElektraIzvrsenjeUsluge racunElektraIzvrsenje in racunElektraIzvrsenjeList)
            {
                racunElektraIzvrsenje.ElektraKupac = _context.ElektraKupac.FirstOrDefault(o => o.Id == racunElektraIzvrsenje.ElektraKupacId);
                racunElektraIzvrsenje.Dopis = _context.Dopis.FirstOrDefault(o => o.Id == racunElektraIzvrsenje.DopisId);

                if (racunElektraIzvrsenje.Dopis != null)
                {
                    racunElektraIzvrsenje.Dopis.Predmet = _context.Predmet.FirstOrDefault(o => o.Id == racunElektraIzvrsenje.Dopis.PredmetId);
                }
            }
            return racunElektraIzvrsenjeList;
        }
    }
}
