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
    public class RacunElektraIzvrsenjeUsluge : Elektra
    {
        [MaxLength(64)]
        public string Usluga { get; set; }

        [Display(Name = "Datum izvršenja")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DatumIzvrsenja { get; set; }
    }
}
