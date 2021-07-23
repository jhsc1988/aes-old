using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace aes.Models
{
    public class RacunElektraTemp
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public int RedniBroj { get; set; }
        public string BrojRacuna { get; set; }
        public ElektraKupac ElektraKupac { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DatumIzdavanja { get; set; }
        public double? Iznos { get; set; }
        public int DopisId { get; set; }
        public IdentityUser User;
    }
}