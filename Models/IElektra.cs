using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public abstract class Elektra : Racun
    {
        public ElektraKupac ElektraKupac { get; set; }
        [Required]
        public int? ElektraKupacId { get; set; }
    }
}
