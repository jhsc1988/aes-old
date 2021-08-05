using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace aes.Models
{
    public class RacunOdsIzvrsenjaUsluge : Racun
    {
        public OdsKupac OdsKupac { get; set; }
        [Required]
        public int OdsKupacId { get; set; }

        // required se podrazumijeva jer nije nullable
        [Display(Name = "Datum izvršenja")]
        public DateTime DatumIzvrsenja { get; set; }

        [MaxLength(64)]
        public string Usluga { get; set; }
    }
}
