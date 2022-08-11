using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace aes.Models.HEP
{
    [Index(nameof(Omm), IsUnique = true)]
    public class Ods
    {
        public int Id { get; set; }

        public Stan Stan { get; set; }
        [Required]
        [Display(Name = "Stan ID")]
        public int StanId { get; set; }

        [Required]
        [Remote(action: "OmmValidation", controller: "Ods")]
        [Display(Name = "Obračunsko mjerno mjesto")]
        public int Omm { get; set; }

        [MaxLength(255)]
        public string Napomena { get; set; }

        [Display(Name = "Vrijeme unosa")]
        public DateTime? VrijemeUnosa { get; set; } // nullable mi treba za not required

        public bool IsEditing { get; set; }
    }
}
