using aes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace aes.Models
{
    public class RacunElektra : Racun
    {
        public ElektraKupac ElektraKupac { get; set; }
        public int? ElektraKupacId { get; set; }
    }
}