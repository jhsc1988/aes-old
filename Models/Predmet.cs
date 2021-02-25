using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public class Predmet
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(8)]
        public string Klasa { get; set; }

        [MaxLength(60)]
#nullable enable
        public string? Naziv { get; set; }
#nullable disable

        [Display(Name = "Vrijeme unosa")]
        public DateTime? VrijemeUnosa { get; set; }
    }
}
