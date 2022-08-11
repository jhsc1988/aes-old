using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace aes.Models.Racuni.Elektra
{
    public class RacunElektraRate : Elektra
    {
        [Required]
        [Remote(action: "BrojRacunaValidation", controller: "RacuniElektraRate")]
        [Display(Name = "Broj računa")]
        [MaxLength(19)]
        public override string BrojRacuna { get; set; }

        // required se podrazumijeva jer nije nullable
        [Display(Name = "Razdoblje")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? Razdoblje { get; set; }
    }
}
