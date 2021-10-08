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
    public class RacunHolding : Racun
    {
        public Stan Stan { get; set; }
        [Required]
        public int StanId { get; set; }

    }
}
