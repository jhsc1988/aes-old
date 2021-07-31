using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
