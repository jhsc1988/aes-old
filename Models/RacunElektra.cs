using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace aes.Models
{
    public class RacunElektra : Racun
    {
        public ElektraKupac ElektraKupac { get; set; }
        public int ElektraKupacId { get; set; }


    }
}
