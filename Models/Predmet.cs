using System;
using System.ComponentModel.DataAnnotations;

namespace aes.Models
{
    public class Predmet
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(21)]
        public string Klasa { get; set; }

        [MaxLength(60)]
        public string Naziv { get; set; }

        [Display(Name = "Vrijeme unosa")]
        public DateTime? VrijemeUnosa { get; set; } // nullable mi treba za not required

        public bool Archived { get; set; }
    }
}

