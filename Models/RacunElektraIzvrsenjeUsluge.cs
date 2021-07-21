using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace aes.Models
{
    public class RacunElektraIzvrsenjeUsluge
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(19)]
        [Remote(action: "BrojRacunaValidation", controller: "RacunElektraIzvrsenjaUsluges")]
        public string BrojRacuna { get; set; }

        public ElektraKupac ElektraKupac { get; set; }
        [Required]
        public int ElektraKupacId { get; set; }

        // required se podrazumijeva jer nije nullable
        [Display(Name = "Datum izdavanja")]
        public DateTime DatumIzdavanja { get; set; }

        // required se podrazumijeva jer nije nullable
        [Display(Name = "Datum izvršenja")]
        public DateTime DatumIzvrsenja { get; set; }

        [MaxLength(64)]
        public string Usluga { get; set; }

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
        public DateTime? DatumPotvrde { get; set; } // nullable mi treba za not required

        [Display(Name = "Vrijeme unosa")]
        public DateTime? VrijemeUnosa { get; set; } // nullable mi treba za not required

        [MaxLength(255)]
        public string Napomena { get; set; }
    }
}
