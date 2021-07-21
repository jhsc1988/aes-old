using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace aes.Models
{
    public class ElektraKupac
    {
        public int Id { get; set; }

        [Required]
        [Remote(action: "UgovorniRacunValidation", controller: "ElektraKupci")]
        public long UgovorniRacun { get; set; }

        public Ods Ods { get; set; }

        [Required]
        public int OdsId { get; set; }

        [MaxLength(255)]
        public string Napomena { get; set; }

        [Display(Name = "Vrijeme unosa")]
        public DateTime? VrijemeUnosa { get; set; } // nullable mi treba za not required
    }
}
