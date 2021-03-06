using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public class RacunOdsIzvrsenjaUsluge
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(19)]
        [Remote(action: "BrojRacunaValidation", controller: "RacunOdsIzvrsenjaUsluge")]
        public string BrojRacuna { get; set; }

        public OdsKupac OdsKupac { get; set; }
        [Required]
        public int OdsKupacId { get; set; }

        // required se podrazumijeva jer nije nullable
        [Display(Name = "Datum izdavanja")]
        public DateTime DatumIzdavanja { get; set; }

        // required se podrazumijeva jer nije nullable
        [Display(Name = "Datum izvršenja")]
        public DateTime DatumIzvrsenja { get; set; }

        [MaxLength(64)]
        public string Usluga { get; set; }

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
