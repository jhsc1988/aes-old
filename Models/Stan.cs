using System;
using System.ComponentModel.DataAnnotations;

namespace aes.Models
{
    public class Stan
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "ID")]
        public int StanId { get; set; }

        [Required]
        [Display(Name = "Šifra objekta")]
        public int SifraObjekta { get; set; }

        // nvarchar(64) - nvarchar can store any Unicode data
#nullable enable
        [MaxLength(64)]
        public string? Vrsta { get; set; }
#nullable disable

        [Required]
        [MaxLength(64)]
        public string Adresa { get; set; }

#nullable enable
        [MaxLength(16)]
        public string? Kat { get; set; }

        [MaxLength(32)]
        [Display(Name = "Broj stana")]
        public string? BrojSTana { get; set; }

        [MaxLength(32)]
        public string? Naselje { get; set; }

        [MaxLength(32)]
        public string? Četvrt { get; set; }

        public double? Površina { get; set; }

        [MaxLength(32)]
        [Display(Name = "Status")]
        public string? StatusKorištenja { get; set; }

        [MaxLength(255)]
        public string? Korisnik { get; set; }

        [MaxLength(32)]
        public string? Vlasništvo { get; set; }

        [MaxLength(8)]
        [Display(Name = "Dio nekretnine")]
        public string? DioNekretnine { get; set; }

        [MaxLength(8)]
        public string? Sektor { get; set; }

        [MaxLength(10)]
        public string? Status { get; set; }

        [Display(Name = "Vrijeme unosa")]
        public DateTime? VrijemeUnosa { get; set; }
    }
}