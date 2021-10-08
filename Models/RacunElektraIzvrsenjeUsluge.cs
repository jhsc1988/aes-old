using aes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace aes.Models
{
    public class RacunElektraIzvrsenjeUsluge : Racun
    {
        public ElektraKupac ElektraKupac { get; set; }
        [Required]
        public int ElektraKupacId { get; set; }

        [MaxLength(64)]
        public string Usluga { get; set; }

        [Display(Name = "Datum izvršenja")]
        public DateTime DatumIzvrsenja { get; set; }
    }
}
