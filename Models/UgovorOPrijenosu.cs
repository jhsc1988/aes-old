using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public class UgovorOPrijenosu
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(14)]
        public string BrojUgovora { get; set; }
        public UgovorOKoristenju UgovorOKoristenju { get; set; }

        [Required]
        public int UgovorOKoristenjuId { get; set; }

        // required se podrazumijeva jer nije nullable
        [Display(Name = "Datum prijenosa")]
        [DataType(DataType.Date)]
        public DateTime DatumPrijenosa { get; set; }

        // required se podrazumijeva jer nije nullable
        [Display(Name = "Datum potpisa")]
        [DataType(DataType.Date)]
        public DateTime DatumPotpisa { get; set; }

        [MaxLength(80)]
        public string Kupac { get; set; }

        public long KupacOIB { get; set; }

        public Dopis Dopis { get; set; }
        public int DopisId { get; set; }
        public int RbrUgovora { get; set; }

        public Dopis DopisDostave { get; set; }
        public int DopisDostaveId { get; set; }
        public int RbrDostave { get; set; }

        public DateTime? VrijemeUnosa { get; set; } // nullable mi treba za not required

    }
}
