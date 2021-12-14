using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace aes.Models.Racuni
{
    public class RacunHolding : Racun
    {
        [Required]
        [Remote(action: "BrojRacunaValidation", controller: "BillsHolding")]
        [Display(Name = "Broj računa")]
        [MaxLength(20)]
        public override string BrojRacuna { get; set; }
        public Stan Stan { get; set; }
        [Required]
        public int StanId { get; set; }
    }
}
