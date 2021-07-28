using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace aes.Models
{
    public class RacunElektra : RacunHEP
    {
        // required se podrazumijeva jer nije nullable
        [Display(Name = "Datum Izdavanja")]
        [DataType(DataType.Date)]
        public DateTime DatumIzdavanja { get; set; }
    }
}
