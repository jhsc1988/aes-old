using aes.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace aes.Models
{
    public class RacunOdsIzvrsenjaUsluge : Racun
    {
        public OdsKupac OdsKupac { get; set; }
        [Required]
        public int OdsKupacId { get; set; }

        // required se podrazumijeva jer nije nullable
        [Display(Name = "Datum izvršenja")]
        public DateTime DatumIzvrsenja { get; set; }

        [MaxLength(64)]
        public string Usluga { get; set; }


        public static List<RacunOdsIzvrsenjaUsluge> GetList(int predmetIdAsInt, int dopisIdAsInt, ApplicationDbContext _context)
        {
            List<RacunOdsIzvrsenjaUsluge> racunOdsIzvrsenjeList = new();

            if (predmetIdAsInt == 0 && dopisIdAsInt == 0)
            {
                racunOdsIzvrsenjeList = _context.RacunOdsIzvrsenjaUsluge.Where(e => e.IsItTemp == null).ToList();
            }

            if (predmetIdAsInt != 0)
            {
                racunOdsIzvrsenjeList = dopisIdAsInt == 0
                    ? _context.RacunOdsIzvrsenjaUsluge.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt).ToList()
                    : _context.RacunOdsIzvrsenjaUsluge.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt && x.Dopis.Id == dopisIdAsInt).ToList();
            }


            foreach (RacunOdsIzvrsenjaUsluge racunElektraIzvrsenje in racunOdsIzvrsenjeList)
            {
                racunElektraIzvrsenje.OdsKupac = _context.OdsKupac.FirstOrDefault(o => o.Id == racunElektraIzvrsenje.OdsKupacId);
                racunElektraIzvrsenje.Dopis = _context.Dopis.FirstOrDefault(o => o.Id == racunElektraIzvrsenje.DopisId);

                if (racunElektraIzvrsenje.Dopis != null)
                {
                    racunElektraIzvrsenje.Dopis.Predmet = _context.Predmet.FirstOrDefault(o => o.Id == racunElektraIzvrsenje.Dopis.PredmetId);
                }
            }
            return racunOdsIzvrsenjeList;
        }

        public static List<RacunOdsIzvrsenjaUsluge> GetListCreateList(string userId, ApplicationDbContext _context)
        {
            List<RacunOdsIzvrsenjaUsluge> racunOdsIzvrsenjeList = _context.RacunOdsIzvrsenjaUsluge.Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true).ToList();

            int rbr = 1;
            foreach (RacunOdsIzvrsenjaUsluge e in racunOdsIzvrsenjeList)
            {

                e.OdsKupac = _context.OdsKupac.FirstOrDefault(o => o.SifraKupca == long.Parse(e.BrojRacuna.Substring(0, 10)));

                List<Racun> racunList = new();
                racunList.AddRange(_context.RacunOdsIzvrsenjaUsluge.Where(e => e.IsItTemp == null || false).ToList());
                e.Napomena = CheckIfExistsInPayed(e.BrojRacuna, racunList);
                racunList.Clear();

                racunList.AddRange(_context.RacunOdsIzvrsenjaUsluge.Where(e => e.IsItTemp == true && e.CreatedByUserId == userId).ToList());
                if (e.Napomena is null)
                {
                    e.Napomena = CheckIfExists(e.BrojRacuna, racunList);
                }

                racunList.Clear();

                if (e.OdsKupac != null)
                {
                    e.OdsKupac.Ods = _context.Ods.FirstOrDefault(o => o.Id == e.OdsKupac.OdsId);
                    e.OdsKupac.Ods.Stan = _context.Stan.FirstOrDefault(o => o.Id == e.OdsKupac.Ods.StanId);
                }
                else
                {
                    e.Napomena = "kupac ne postoji";
                }
                e.RedniBroj = rbr++;
            }
            return racunOdsIzvrsenjeList;
        }
    }
}
