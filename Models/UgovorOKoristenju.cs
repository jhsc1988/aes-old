using System;
using System.ComponentModel.DataAnnotations;

namespace aes.Models
{
    public class UgovorOKoristenju
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(14)]
        public string BrojUgovora { get; set; }
        public Ods Ods { get; set; }

        [Required]
        public int OdsId { get; set; }

        // required se podrazumijeva jer nije nullable
        [Display(Name = "Datum potpisa HEP")]
        [DataType(DataType.Date)]
        public DateTime DatumPotpisaHEP { get; set; }

        // required se podrazumijeva jer nije nullable
        [Display(Name = "Datum potpisa GZ")]
        [DataType(DataType.Date)]
        public DateTime DatumPotpisaGZ { get; set; }

        public Dopis Dopis { get; set; }
        public int DopisId { get; set; }
        public int RbrUgovora { get; set; }

        public Dopis DopisDostave { get; set; }
        public int DopisDostaveId { get; set; }
        public int RbrDostave { get; set; }

        public DateTime? VrijemeUnosa { get; set; } // nullable mi treba za not required
    }
}
