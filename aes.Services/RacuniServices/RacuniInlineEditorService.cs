using aes.Models.Racuni;
using aes.Services.RacuniServices.IRacuniService;
using aes.Services.RacuniServices.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using aes.UnitOfWork;

namespace aes.Services.RacuniServices
{
    public class RacuniInlineEditorService : IRacuniInlineEditorService
    {
        private readonly IService _service;
        private readonly IUnitOfWork _unitOfWork;

        public RacuniInlineEditorService(IService service, IUnitOfWork unitOfWork)
        {
            _service = service;
            _unitOfWork = unitOfWork;
        }

        public async Task<JsonResult> UpdateDbForInline<T>(Racun RacunToUpdate, string updatedColumn, string x) where T : Racun
        {
            Columns column = (Columns)int.Parse(updatedColumn);

            switch (column)
            {
                case Columns.Racun:
                    if (x.Length < 10)
                    {
                        return new(new { success = false, Message = "Broj računa nije ispravan!" });
                    }

                    if (!x[..10].Equals(RacunToUpdate.BrojRacuna[..10]))
                    {
                        return new(new { success = false, Message = "Pogrešan broj računa - ugovorni računi ne smije se razlikovati!" });
                    }

                    RacunToUpdate.BrojRacuna = x;
                    break;
                case Columns.DatumIzdavanja:
                    RacunToUpdate.DatumIzdavanja = x == null ? null : DateTime.Parse(x);
                    break;
                case Columns.Iznos:
                    string iznosNumeric = new(x.Where(char.IsDigit).ToArray());
                    RacunToUpdate.Iznos = decimal.Parse(iznosNumeric);
                    break;
                case Columns.Klasa:
                    RacunToUpdate.KlasaPlacanja = x;
                    RacunToUpdate.DatumPotvrde = DateTime.Now;
                    //if (racunToUpdate.KlasaPlacanja is null && racunToUpdate.DatumPotvrde is not null)
                    //{
                    //    racunToUpdate.DatumPotvrde = null;
                    //}

                    break;
                case Columns.DatumPotvrde:
                    //if (racunToUpdate.KlasaPlacanja is null)
                    //{
                    //    return new(new { success = false, Message = "Ne mogu evidentirati datum potvrde bez klase plaćanja!" });
                    //}
                    //else
                    {
                        RacunToUpdate.DatumPotvrde = x == null ? null : DateTime.Parse(x);
                    }
                    break;
                case Columns.Napomena:
                    RacunToUpdate.Napomena = x;
                    break;
                case Columns.DatumIzvrsenja:
                    (await _unitOfWork.RacuniElektraIzvrsenjeUsluge.Get(RacunToUpdate.Id)).DatumIzvrsenja = DateTime.Parse(x);
                    break;
                case Columns.Usluga:
                    (await _unitOfWork.RacuniElektraIzvrsenjeUsluge.Get(RacunToUpdate.Id)).Usluga = x;
                    break;
                default:
                    break;
            }
            return await _service.TrySave(false);
        }
    }
}
