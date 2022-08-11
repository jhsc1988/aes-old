using aes.Models.Racuni.Elektra;
using System;
using System.ComponentModel.DataAnnotations;

namespace aes.Models.HEP
{
    public class ObracunPotrosnje
    {
        public int Id { get; set; }
        public RacunElektra RacunElektra { get; set; }
        public int RacunElektraId { get; set; }

        [Display(Name = "Broj brojila")]
        public long BrojBrojila { get; set; }
        public TarifnaStavka TarifnaStavka { get; set; }

        [Display(Name = "Tarifa")]
        public int TarifnaStavkaId { get; set; }

        [Display(Name = "Datum od")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DatumOd { get; set; }

        [Display(Name = "Datum do")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DatumDo { get; set; }

        [Display(Name = "Stanje od")]
        public double StanjeOd { get; set; }

        [Display(Name = "Stanje od")]
        public double StanjeDo { get; set; }
    }
}
