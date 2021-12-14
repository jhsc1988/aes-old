﻿using aes.CommonDependecies;
using aes.Models.Racuni;
using aes.Services.BillsServices.BillsHoldingService.IService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.BillsHoldingService
{
    public class BillsHoldingTempCreateService : IBillsHoldingTempCreateService
    {
        private readonly IBillsCommonDependecies _c;

        public BillsHoldingTempCreateService(IBillsCommonDependecies c)
        {
            _c = c;
        }

        public async Task<JsonResult> AddNewTemp(string brojRacuna, string iznos, string datumIzdavanja, string userId)
        {

            if (iznos == null)
            {
                return new(new { success = false, Message = "iznos ne smije biti prazan!" });
            }
            if ((await _c.UnitOfWork.BillsHolding.TempList(userId)).Count() >= 500)
            {
                return new(new { success = false, Message = "U tablici ne može biti više od 500 računa!" });
            }

            double _iznos = double.Parse(iznos);
            DateTime? _datumIzdavanja = datumIzdavanja is not null ? DateTime.Parse(datumIzdavanja) : null;

            RacunHolding re = new()
            {
                BrojRacuna = brojRacuna,
                Iznos = _iznos,
                DatumIzdavanja = _datumIzdavanja,
                CreatedByUserId = userId,
                IsItTemp = true,
            };

            if (re.BrojRacuna.Length >= 8)
            {
                re.Stan = await _c.UnitOfWork.Apartment.FindExact(e => e.SifraObjekta == int.Parse(re.BrojRacuna.Substring(0, 8)));
                if (re.Stan == null)
                {
                    re.Stan = await _c.UnitOfWork.Apartment.FindExact(e => e.Id == 25265);
                }
            }
            else
            {
                re.Stan = await _c.UnitOfWork.Apartment.FindExact(e => e.Id == 25265);
            }

            IEnumerable<RacunHolding> tempRacuni = (await _c.UnitOfWork.BillsHolding.TempList(userId)).Append(re);

            int rbr = 1;
            foreach (RacunHolding e in tempRacuni)
            {
                e.RedniBroj = rbr++;
            }
            _c.UnitOfWork.BillsHolding.Add(re);
            return await _c.Service.TrySave(false);
        }

        public async Task<int> CheckTempTableForBillsWithouCustomer(string userId)
        {
            IEnumerable<RacunHolding> list = await _c.UnitOfWork.BillsHolding.TempList(userId);
            return list.Count(e => e.StanId == 25265);
        }

        public async Task<JsonResult> RefreshCustomers(string userId)
        {
            IEnumerable<RacunHolding> tempRacuni = await _c.UnitOfWork.BillsHolding.TempList(userId);

            if (tempRacuni.Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }

            foreach (RacunHolding e in tempRacuni)
            {
                if (e.StanId == 25265 && e.BrojRacuna.Length >= 8)
                {
                    e.Stan = await _c.UnitOfWork.BillsHolding.GetApartmentBySifraObjekta(int.Parse(e.BrojRacuna[..8]));
                    if (e.Stan == null)
                    {
                        e.Stan = await _c.UnitOfWork.Apartment.FindExact(e => e.Id == 25265);
                    }
                }
            }
            await BillsElektraNotesBuild(userId, tempRacuni);
            return new(new { success = true, Message = "Podaci su osvježeni" });
        }

        private async Task BillsElektraNotesBuild(string userId, IEnumerable<RacunHolding> list)
        {
            foreach (RacunHolding e in list)
            {
                if (e.StanId == 25265)
                {
                    e.Napomena = "kupac ne postoji";
                }
                else
                {
                    IEnumerable<RacunHolding> bills = await _c.UnitOfWork.BillsHolding.GetBillsForApartment(e.StanId);
                    e.Napomena = await _c.BillsCheckService.CheckIfExistsInPayed(e.BrojRacuna, bills);
                }

                if (e.Napomena is null)
                {
                    IEnumerable<RacunHolding> tempBills = await _c.UnitOfWork.BillsHolding.Find(item => item.IsItTemp == true && item.CreatedByUserId == userId && item.StanId == e.StanId);
                    e.Napomena = await _c.BillsCheckService.CheckIfExistsInTemp(e.BrojRacuna, tempBills);
                }

                _ = await _c.UnitOfWork.BillsHolding.Update(e);
            }
        }
    }

}