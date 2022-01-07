using aes.Models.Racuni;
using aes.Repository.UnitOfWork;
using aes.Services.BillsServices.IBillsService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Services.BillsServices
{
    public class BillsInlineEditorService : IBillsInlineEditorService
    {
        private readonly IService _service;
        private readonly IUnitOfWork _unitOfWork;

        public BillsInlineEditorService(IService service, IUnitOfWork unitOfWork)
        {
            _service = service;
            _unitOfWork = unitOfWork;
        }

        public async Task<JsonResult> UpdateDbForInline<T>(Racun billToUpdate, string updatedColumn, string x) where T : Racun
        {
            Columns column = (Columns)int.Parse(updatedColumn);

            switch (column)
            {
                case Columns.racun:
                    if (x.Length < 10)
                    {
                        return new(new { success = false, Message = "Broj računa nije ispravan!" });
                    }

                    if (!x[..10].Equals(billToUpdate.BrojRacuna[..10]))
                    {
                        return new(new { success = false, Message = "Pogrešan broj računa - ugovorni računi ne smije se razlikovati!" });
                    }

                    billToUpdate.BrojRacuna = x;
                    break;
                case Columns.datumIzdavanja:
                    billToUpdate.DatumIzdavanja = x == null ? null : DateTime.Parse(x);
                    break;
                case Columns.iznos:
                    string iznosNumeric = new(x.Where(char.IsDigit).ToArray());
                    billToUpdate.Iznos = decimal.Parse(iznosNumeric);
                    break;
                case Columns.klasa:
                    billToUpdate.KlasaPlacanja = x;
                    billToUpdate.DatumPotvrde = DateTime.Now;
                    //if (racunToUpdate.KlasaPlacanja is null && racunToUpdate.DatumPotvrde is not null)
                    //{
                    //    racunToUpdate.DatumPotvrde = null;
                    //}

                    break;
                case Columns.datumPotvrde:
                    //if (racunToUpdate.KlasaPlacanja is null)
                    //{
                    //    return new(new { success = false, Message = "Ne mogu evidentirati datum potvrde bez klase plaćanja!" });
                    //}
                    //else
                    {
                        billToUpdate.DatumPotvrde = x == null ? null : DateTime.Parse(x);
                    }
                    break;
                case Columns.napomena:
                    billToUpdate.Napomena = x;
                    break;
                case Columns.datumIzvrsenja:
                    (await _unitOfWork.BillsElektraServices.Get(billToUpdate.Id)).DatumIzvrsenja = DateTime.Parse(x);
                    break;
                case Columns.usluga:
                    (await _unitOfWork.BillsElektraServices.Get(billToUpdate.Id)).Usluga = x;
                    break;
                default:
                    break;
            }
            return await _service.TrySave(false);
        }
    }
}
