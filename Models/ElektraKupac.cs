using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace aes.Models
{
    public class ElektraKupac : Kupac
    {
        [Required]
        [Remote(action: "UgovorniRacunValidation", controller: "ElektraKupci")]
        public long UgovorniRacun { get; set; }

        public Ods Ods { get; set; }

        [Required]
        public int OdsId { get; set; }
    }

}
