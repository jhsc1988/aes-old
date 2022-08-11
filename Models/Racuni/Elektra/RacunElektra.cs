using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace aes.Models.Racuni.Elektra
{
    public class RacunElektra : Elektra
    {
        [Required]
        [Remote(action: "BrojRacunaValidation", controller: "RacuniElektra")]
        [Display(Name = "Broj računa")]
        [MaxLength(19)]
        public override string BrojRacuna { get; set; }
    }
}