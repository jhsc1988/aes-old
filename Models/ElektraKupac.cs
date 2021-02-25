using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public class ElektraKupac
    {
        public int Id { get; set; }

        [Required]
        [Remote(action: "UgovorniRacun", controller: "ElektraKupac")]
        public int UgovorniRacun { get; set; }
        
        public Ods Ods { get; set; }
        
        [Required]
        public int OdsId { get; set; }

        [MaxLength(255)]
        public string Napomena { get; set; }

        [Display(Name = "Vrijeme unosa")]
        public DateTime? VrijemeUnosa { get; set; }
    }
}
