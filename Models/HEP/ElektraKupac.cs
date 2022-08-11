using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace aes.Models.HEP
{
    public class ElektraKupac : Kupac
    {
        [Required]
        [Display(Name = "Ugovorni račun")]
        [Remote(action: "UgovorniRacunValidation", controller: "ElektraKupci")]
        public long UgovorniRacun { get; set; }

        public Ods Ods { get; set; }

        [Required]
        [Display(Name = "ODS ID")]
        public int OdsId { get; set; }
    }

}
