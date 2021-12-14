using System;
using System.ComponentModel.DataAnnotations;

namespace aes.Models
{
    public abstract class Kupac
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Napomena { get; set; }

        [Display(Name = "Vrijeme unosa")]
        public DateTime? VrijemeUnosa { get; set; } // nullable mi treba za not required
    }
}
