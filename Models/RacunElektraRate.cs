﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public class RacunElektraRate
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        [Remote(action: "BrojRacuna", controller: "RacunElektra")]
        public string BrojRacuna { get; set; }

        [Required]
        public ElektraKupac ElektraKupac { get; set; }
        public int ElektraKupacId { get; set; }

        [Required]
        [Display(Name = "Razdoblje")]
        public DateTime Razdoblje { get; set; }

        [Required]
        public double Iznos { get; set; }

        [Required]
        public Dopis Dopis { get; set; }
        public int DopisId { get; set; }

        [Required]
        public int RedniBroj { get; set; }

        [MaxLength(20)]
        [Display(Name = "Klasa Plaćanja")]
#nullable enable
        public string? KlasaPlacanja { get; set; }
#nullable disable
        [Display(Name = "Datum Potvrde")]
        public DateTime? DatumPotvrde { get; set; }

        [Display(Name = "Vrijeme unosa")]
        public DateTime? VrijemeUnosa { get; set; }
    }
}
