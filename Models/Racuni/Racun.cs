using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aes.Models.Racuni
{
    public abstract class Racun
    {
        public abstract string BrojRacuna { get; set; }

        public int Id { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Iznos { get; set; }

        [Display(Name = "Datum izdavanja")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? DatumIzdavanja { get; set; }

        public Dopis Dopis { get; set; }

        public int? DopisId { get; set; }

        [Required]
        [Display(Name = "Redni broj")]
        public int RedniBroj { get; set; }

        [MaxLength(20)]
        [Display(Name = "Klasa plaćanja")]
        public string KlasaPlacanja { get; set; }

        [Display(Name = "Datum potvrde")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? DatumPotvrde { get; set; }

        [Display(Name = "Vrijeme unosa")]
        [DataType(DataType.Date)]
        public DateTime? VrijemeUnosa { get; set; }

        [MaxLength(255)]
        public string Napomena { get; set; }

        public bool? IsItTemp { get; set; }

        public string CreatedByUserId { get; set; }
    }
}