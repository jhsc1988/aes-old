using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public class Dopis
    {
        public int Id { get; set; }

        public Predmet Predmet { get; set; }
        public int PredmetId { get; set; }

        [Required]
        [MaxLength(25)]
        public string Urbroj { get; set; }

        public DateTime? Datum { get; set; }

        [Display(Name = "Vrijeme unosa")]
        public DateTime? VrijemeUnosa { get; set; }
    }
}
