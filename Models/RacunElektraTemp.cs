using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    [Table("RacuniElektraTemp")]
    public class RacunElektraTemp : RacunElektra
    {
        public Guid Guid { get; set; }
        public string UserId { get; set; }
    }
}
