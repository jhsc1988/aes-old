using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    [Index(nameof(Omm), IsUnique = true)]
    public class Ods
    {
        public int Id { get; set; }

        public Stan Stan { get; set; }
        [Required]
        public int StanId { get; set; }

        [Required]
        [Remote(action: "OmmValidation", controller: "Ods")]
        public int Omm { get; set; }

        [MaxLength(255)]
        public string Napomena { get; set; }

        [Display(Name = "Vrijeme unosa")]
        public DateTime? VrijemeUnosa { get; set; } // nullable mi treba za not required
    }
}
