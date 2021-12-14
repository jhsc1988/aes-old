﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace aes.Models.Racuni
{
    public class RacunElektraIzvrsenjeUsluge : Elektra
    {
        [Required]
        [Remote(action: "BrojRacunaValidation", controller: "BillsElektraServices")]
        [Display(Name = "Broj računa")]
        [MaxLength(19)]
        public override string BrojRacuna { get; set; }

#nullable enable
        [MaxLength(64)]
        public string? Usluga { get; set; }
#nullable disable

        [Display(Name = "Datum izvršenja")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? DatumIzvrsenja { get; set; }
    }
}
