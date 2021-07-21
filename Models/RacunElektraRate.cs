using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace aes.Models
{
    public class RacunElektraRate
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(19)]
        [Remote(action: "BrojRacunaValidation", controller: "RacuniElektraRate")]
        public string BrojRacuna { get; set; }

        public ElektraKupac ElektraKupac { get; set; }
        [Required]
        public int ElektraKupacId { get; set; }

        // required se podrazumijeva jer nije nullable
        [Display(Name = "Razdoblje")]
        [DataType(DataType.Date)]
        public DateTime Razdoblje { get; set; }

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
    }
}
