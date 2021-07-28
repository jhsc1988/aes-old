using aes.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using aes.Data;
using aes.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;
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

        public Dopis Dopis { get; set; }
        [Required]
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

        public ClaimsPrincipal CreatedBy { get; set; }

        public Racun()
        {
            CreatedBy = ClaimsPrincipal.Current;
        }
        public static bool CheckIfExistsInPayed(string brojRacuna, List<Racun> racunList)
        {
            int num = racunList.Where(x => x.BrojRacuna.Equals(brojRacuna)).Count();
            return num >= 1;
        }
    }
}
