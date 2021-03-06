using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public class RacunElektraObracunPotrosnje
    {
        public int Id { get; set; }

        // required se podrazumijeva jer nije nullable
        [Display(Name = "Datum obračuna")]
        public DateTime DatumObracuna { get; set; }

        [Required]
        [MaxLength(9)]
        public string brojilo { get; set; }

        [Required]
        [Remote(action: "BrojRacuna", controller: "ElektraRacunObracunPotrosnje")]
        [Display(Name = "Broj računa")]
        public int BrojRacuna { get; set; }

        [Required]
        public int RVT { get; set; }
        public int RNT { get; set; }

        [Display(Name = "Vrijeme unosa")]
        public DateTime? VrijemeUnosa { get; set; } // nullable mi treba za not required
    }
}
