using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace aes.Models
{
    public class ElektraKupac : Kupac
    {
        [Required]
        [Display(Name = "Ugovorni račun")]
        [Remote(action: "UgovorniRacunValidation", controller: "ElektraCustomers")]
        public long UgovorniRacun { get; set; }

        public Ods Ods { get; set; }

        [Required]
        [Display(Name = "ODS ID")]
        public int OdsId { get; set; }
    }

}
