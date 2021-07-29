using aes.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using aes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using System.Text.Json;
namespace aes.Models
{
    public abstract class Racun
    {
        public int Id { get; set; }

        [Required]
        [Remote(action: "BrojRacunaValidation", controller: "RacuniElektra")]
        [MaxLength(19)]
        public string BrojRacuna { get; set; }

        // TODO: postaviti decimal za money
        // [DataType(DataType.Currency)]
        [Required]
        public double Iznos { get; set; }

        // required se podrazumijeva jer nije nullable
        [Display(Name = "Datum Izdavanja")]
        [DataType(DataType.Date)]

        public DateTime? DatumIzdavanja { get; set; }
        public Dopis Dopis { get; set; }
        public int DopisId { get; set; }

        [Required]
        public int RedniBroj { get; set; }

        [MaxLength(20)]
        [Display(Name = "Klasa Plaćanja")]
        public string KlasaPlacanja { get; set; }

        [Display(Name = "Datum Potvrde")]
        [DataType(DataType.Date)]
        public DateTime? DatumPotvrde { get; set; } // nullable mi treba za not required

        [Display(Name = "Vrijeme unosa")]
        [DataType(DataType.Date)]
        public DateTime? VrijemeUnosa { get; set; } // nullable mi treba za not required

        [MaxLength(255)]
        public string Napomena { get; set; }

        public bool? IsItTemp { get; set; }

        public string CreatedByUserId { get; set; }

        public static bool CheckIfExists(string brojRacuna, List<Racun> racunList)
        {
            int numOfOccurrences = racunList.Where(x => x.BrojRacuna.Equals(brojRacuna)).Count();
            return numOfOccurrences >= 2;
        }

        public static bool CheckIfExistsInPayed(string brojRacuna, List<Racun> racunList)
        {
            int numOfOccurrences = racunList.Where(x => x.BrojRacuna.Equals(brojRacuna)).Count();
            return numOfOccurrences >= 1;
        }

        public static bool RemoveRow(int racunId, List<Racun> racunList)
        {
            return racunList.Remove(racunList.FirstOrDefault(e => e.Id == racunId));
        }

        public static bool Validate(string brojRacuna, string iznos, string date, string dopisId, out string msg, out double _iznos, out int _dopisId, out DateTime? datumIzdavanja)
        {
            _dopisId = 0;
            datumIzdavanja = null;
            msg = null;
            DateTime dt;
           
            if (!double.TryParse(iznos, out _iznos))
            {
                msg = "Iznos je neispravan";
                return false;
            }

            if (!int.TryParse(dopisId, out _dopisId))
            {
                msg = "Dopis ID je neispravan";
                return false;
            }

            if (brojRacuna == null)
            {
                msg = "Broj računa je obavezan";
                return false;
            }

            if (date == null)
            {
                msg = "Datum izdavanja je obavezan";
                return false;
            }
            else if (!DateTime.TryParse(date, out dt))
            {
                msg = "Datum izdavanja je obavezan";
                return false;
            }

            if (double.Parse(iznos) <= 0)
            {
                msg = "Iznos mora biti veći od 0 kn";
                return false;
            }

            datumIzdavanja = dt;
            return true;
        }
    }
}

