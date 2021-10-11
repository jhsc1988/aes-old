using aes.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;


namespace aes.Models.Workshop
{
    public class RacunWorkshop : Workshop, IRacunWorkshop
    {
        /// <summary>
        /// Columns in Index - columns definitions for inline editor
        /// </summary>
        private enum Columns
        {
            racun = 1, datumIzdavanja = 2, iznos = 3, klasa = 4, datumPotvrde = 5, napomena = 6, datumIzvrsenja = 7, usluga = 8
        }

        public string CheckIfExists(string brojRacuna, List<Racun> racunList)
        {
            int numOfOccurrences = racunList.Where(x => x.BrojRacuna.Equals(brojRacuna)).Count();
            return numOfOccurrences >= 2 ? "dupli račun" : null;
        }

        public string CheckIfExistsInPayed(string brojRacuna, List<Racun> racunList)
        {
            int numOfOccurrences = racunList.Where(x => x.BrojRacuna.Equals(brojRacuna)).Count();
            return numOfOccurrences >= 1 ? "račun već plaćen" : null;
        }

        public JsonResult RemoveRow<T>(string racunId, DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun
        {
            int id = int.Parse(racunId);
            _ = _context.Remove(_modelcontext.FirstOrDefault(x => x.Id == id));
            return TrySave(_context, true);
        }

        public bool Validate(string brojRacuna, string iznos, string date, string dopisId, out string msg, out double _iznos, out int _dopisId, out DateTime? datumIzdavanja)
        {
            _iznos = 0; datumIzdavanja = null; msg = null; DateTime dt;

            if (!int.TryParse(dopisId, out _dopisId)) { msg = "Dopis ID je neispravan"; return false; }
            if (brojRacuna == null) { msg = "Broj računa je obavezan"; return false; }
            if (date == null) { msg = "Datum izdavanja je obavezan"; return false; }
            else if (!DateTime.TryParse(date, out dt)) { msg = "Datum izdavanja je obavezan"; return false; }

            // "en-US" mi treba za decimalnu tocku, decimanlna točka mi treba za Excel export
            if (!double.TryParse(iznos, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out _iznos)) { msg = "Iznos je neispravan"; return false; }
            if (double.Parse(iznos) <= 0) { msg = "Iznos mora biti veći od 0 kn"; return false; }

            datumIzdavanja = dt;
            return true;
        }
        public JsonResult UpdateDbForInline<T>(string racunId, string updatedColumn, string x, DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun
        {
            int idNum = int.Parse(racunId);
            Racun racunToUpdate = null;
            Columns column = (Columns)int.Parse(updatedColumn);
            racunToUpdate = _modelcontext.First(e => e.Id == idNum);

            switch (column)
            {
                case Columns.racun:
                    if (x.Length < 10) return new(new { success = false, Message = "Broj računa nije ispravan!" });
                    if (!x.Substring(0, 10).Equals(racunToUpdate.BrojRacuna.Substring(0, 10))) return new(new { success = false, Message = "Pogrešan broj računa - ugovorni računi ne smije se razlikovati!" });
                    racunToUpdate.BrojRacuna = x;
                    break;
                case Columns.datumIzdavanja:
                    racunToUpdate.DatumIzdavanja = DateTime.Parse(x);
                    break;
                case Columns.iznos:
                    racunToUpdate.Iznos = double.Parse(x);
                    break;
                case Columns.klasa:
                    racunToUpdate.KlasaPlacanja = x;
                    if (racunToUpdate.KlasaPlacanja is null && racunToUpdate.DatumPotvrde is not null) racunToUpdate.DatumPotvrde = null;
                    break;
                case Columns.datumPotvrde:
                    if (racunToUpdate.KlasaPlacanja is null) return new(new { success = false, Message = "Ne mogu evidentirati datum potvrde bez klase plaćanja!" });
                    else racunToUpdate.DatumPotvrde = DateTime.Parse(x);
                    break;
                case Columns.napomena:
                    racunToUpdate.Napomena = x;
                    break;
                case Columns.datumIzvrsenja:
                    _context.RacunElektraIzvrsenjeUsluge.First(e => e.Id == idNum).DatumIzvrsenja = DateTime.Parse(x);
                    break;
                case Columns.usluga:
                    _context.RacunElektraIzvrsenjeUsluge.First(e => e.Id == idNum).Usluga = x;
                    break;
                default:
                    break;
            }
            return TrySave(_context, false);
        }
        public JsonResult SaveToDb<T>(string userId, string _dopisId, DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun
        {
            List<Racun> racunList = new();
            int dopisId = int.Parse(_dopisId);
            if (dopisId is 0) return new(new { success = false, Message = "Nije odabran dopis!" });
            racunList.AddRange(_modelcontext.Where(e => e.IsItTemp == true && e.CreatedByUserId.Equals(userId)).ToList());
            foreach (Racun e in racunList)
            {
                e.IsItTemp = null;
                e.DopisId = dopisId;
            }
            return TrySave(_context, false);
        }
        public JsonResult RemoveAllFromDbTemp<T>(string userId, DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun
        {
            _context.RemoveRange(_modelcontext.Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true));
            return TrySave(_context, true);
        }

        public List<T> GetRacuniFromDb<T>(DbSet<T> modelcontext, int param = 0) where T : Elektra
        => param switch
        {
            not 0 => modelcontext
                            .Include(e => e.ElektraKupac)
                            .Include(e => e.ElektraKupac.Ods)
                            .Include(e => e.ElektraKupac.Ods.Stan)
                            .Where(e => e.ElektraKupac.Id == param)
                            .ToList(),
            _ => modelcontext
                    .Include(e => e.ElektraKupac)
                    .Include(e => e.ElektraKupac.Ods)
                    .Include(e => e.ElektraKupac.Ods.Stan)
                    .ToList()
        };
        public string GetUid(ClaimsPrincipal User) => User.FindFirstValue(ClaimTypes.NameIdentifier);
        public List<T> GetListCreateList<T>(string userId, ApplicationDbContext _context, DbSet<T> modelcontext) where T : Elektra
        {
            List<T> list = modelcontext.Where(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true).ToList();

            int rbr = 1;
            foreach (Elektra e in list)
            {
                e.ElektraKupac = _context.ElektraKupac.FirstOrDefault(o => o.UgovorniRacun == long.Parse(e.BrojRacuna.Substring(0, 10)));

                List<Racun> racunList = new();
                racunList.AddRange(modelcontext.Where(e => e.IsItTemp == null || false).ToList());
                e.Napomena = CheckIfExistsInPayed(e.BrojRacuna, racunList);
                racunList.Clear();

                racunList.AddRange(modelcontext.Where(e => e.IsItTemp == true && e.CreatedByUserId == userId).ToList());
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
            return list;
        }
        public JsonResult GetListMe<T>(bool isFiltered, string klasa, string urbroj, IDatatablesGenerator datatablesGenerator,
            ApplicationDbContext _context, IRacunWorkshop workshop, DbSet<T> modelcontext, HttpRequest Request, string Uid) where T : Elektra
        {
            IDatatablesParams Params = datatablesGenerator.GetParams(Request);
            List<T> list;
            if (isFiltered)
            {
                int predmetIdAsInt = klasa is null ? 0 : int.Parse(klasa);
                int dopisIdAsInt = urbroj is null ? 0 : int.Parse(urbroj);
                list = GetList(predmetIdAsInt, dopisIdAsInt, modelcontext);
            }
            else
            {
                list = workshop.GetListCreateList(Uid, _context, modelcontext);
            }

            int totalRows = list.Count;
            if (!string.IsNullOrEmpty(Params.SearchValue))
            {
                switch (list)
                {
                    case List<RacunElektra>:
                        list = new RacunElektraWorkshop().GetRacuniElektraForDatatables(Params, list as List<RacunElektra>) as List<T>;
                        break;
                    case List<RacunElektraRate>:
                        list = new RacunElektraRateWorkshop().GetRacuniElektraRateForDatatables(Params, list as List<RacunElektraRate>) as List<T>;
                        break;
                    case List<RacunElektraIzvrsenjeUsluge>:
                        list = new RacunElektraIzvrsenjeUslugeWorkshop().GetRacunElektraIzvrsenjeUslugeForDatatables(Params, list as List<RacunElektraIzvrsenjeUsluge>) as List<T>;
                        break;
                    default:
                        break;
                }
            }
            int totalRowsAfterFiltering = list.Count;
            return datatablesGenerator.SortingPaging(list, Params, Request, totalRows, totalRowsAfterFiltering);
        }
        public List<T> GetList<T>(int predmetIdAsInt, int dopisIdAsInt, DbSet<T> modelcontext) where T : Elektra
        {
            IQueryable<T> RacunElektraRateList = predmetIdAsInt is 0 && dopisIdAsInt is 0
                ? modelcontext.Where(e => e.IsItTemp == null)
                : dopisIdAsInt is 0
                    ? modelcontext.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt)
                    : modelcontext.Where(x => x.Dopis.Predmet.Id == predmetIdAsInt && x.Dopis.Id == dopisIdAsInt);
            return GetRacuniFromDb(modelcontext);
        }
    }
}

