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

        [MaxLength(64)]
        public string Usluga { get; set; }

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

        public static List<RacunElektraIzvrsenjeUsluge> GetListCreateList(string userId, ApplicationDbContext _context)
        {
            List<RacunElektraIzvrsenjeUsluge> racunElektraIzvrsenjeList = _context.RacunElektraIzvrsenjeUsluge.Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true).ToList();

            int rbr = 1;
            foreach (RacunElektraIzvrsenjeUsluge e in racunElektraIzvrsenjeList)
            {

                e.ElektraKupac = _context.ElektraKupac.FirstOrDefault(o => o.UgovorniRacun == long.Parse(e.BrojRacuna.Substring(0, 10)));

                List<Racun> racunList = new();
                racunList.AddRange(_context.RacunElektraIzvrsenjeUsluge.Where(e => e.IsItTemp == null || false).ToList());
                e.Napomena = CheckIfExistsInPayed(e.BrojRacuna, racunList);
                racunList.Clear();

                racunList.AddRange(_context.RacunElektraIzvrsenjeUsluge.Where(e => e.IsItTemp == true && e.CreatedByUserId == userId).ToList());
                if (e.Napomena is null)
                {
                    e.Napomena = CheckIfExists(e.BrojRacuna, racunList);
                }

                racunList.Clear();

                if (e.ElektraKupac != null)
                {
                    e.ElektraKupac.Ods = _context.Ods.FirstOrDefault(o => o.Id == e.ElektraKupac.OdsId);
                    e.ElektraKupac.Ods.Stan = _context.Stan.FirstOrDefault(o => o.Id == e.ElektraKupac.Ods.StanId);
                }
                else
                {
                    e.Napomena = "kupac ne postoji";
                }
                e.RedniBroj = rbr++;
            }
            return racunElektraIzvrsenjeList;
        }

        public static JsonResult AddNewTemp(string brojRacuna, string iznos, string datumPotvrde, string datumIzvrsenja, string usluga, string dopisId, string userId, ApplicationDbContext _context)
        {

            DateTime datumIzvrsenjaDT = new();
            if (usluga == null)
            {
                return new(new { success = false, Message = "Usluga je obavezna" });
            }

            if (datumIzvrsenja != null)
            {
                datumIzvrsenjaDT = DateTime.Parse(datumIzvrsenja);
            }

            if (!Validate(brojRacuna, iznos, datumPotvrde, dopisId, out string msg, out double _iznos, out int _dopisId, out DateTime? datumIzdavanja))
                return new(new { success = false, Message = msg });

            List<RacunElektraIzvrsenjeUsluge> racunElektraIzvrsenjeList = _context.RacunElektraIzvrsenjeUsluge.Where(e => e.IsItTemp == true && e.CreatedByUserId.Equals(userId)).ToList();
            RacunElektraIzvrsenjeUsluge re = new()
            {
                BrojRacuna = brojRacuna,
                Iznos = _iznos,
                DatumIzdavanja = datumIzdavanja,
                DatumIzvrsenja = datumIzvrsenjaDT,
                Usluga = usluga,
                DopisId = _dopisId == 0 ? null : _dopisId,
                CreatedByUserId = userId,
                IsItTemp = true,
            };

            re.ElektraKupac = _context.ElektraKupac.FirstOrDefault(o => o.UgovorniRacun == long.Parse(re.BrojRacuna.Substring(0, 10)));
            racunElektraIzvrsenjeList.Add(re);

            int rbr = 1;
            foreach (RacunElektraIzvrsenjeUsluge e in racunElektraIzvrsenjeList)
            {
                e.RedniBroj = rbr++;
            }
            _ = _context.RacunElektraIzvrsenjeUsluge.Add(re);

            return TrySave(_context);
        }
    }
}
