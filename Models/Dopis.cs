using System;
using System.ComponentModel.DataAnnotations;

namespace aes.Models
{
    public class Dopis
    {
        public int Id { get; set; }

        public Predmet Predmet { get; set; }
        public int PredmetId { get; set; }

        [Required]
        [MaxLength(25)]
        public string Urbroj { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? Datum { get; set; }

        [Display(Name = "Vrijeme unosa")]
        public DateTime? VrijemeUnosa { get; set; }
    }
}
