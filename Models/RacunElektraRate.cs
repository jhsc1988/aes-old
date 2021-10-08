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
    public class RacunElektraRate : Racun
    {
        public ElektraKupac ElektraKupac { get; set; }
        [Required]
        public int? ElektraKupacId { get; set; }

        // required se podrazumijeva jer nije nullable
        [Display(Name = "Razdoblje")]
        [DataType(DataType.Date)]
        public DateTime Razdoblje { get; set; }
    }
}
