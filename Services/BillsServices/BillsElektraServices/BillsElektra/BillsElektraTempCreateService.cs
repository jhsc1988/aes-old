﻿using aes.CommonDependecies;
using aes.Models.Racuni;
using aes.Services.BillsServices.BillsElektraServices.BillsElektra.Is;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.BillsElektraServices.BillsElektra
{
    public class BillsElektraTempCreateService : IBillsElektraTempCreateService
    {
        private readonly IBillsCommonDependecies _c;

        public BillsElektraTempCreateService(IBillsCommonDependecies c)
        {
            _c = c;
        }
        public async Task<JsonResult> AddNewTemp(string brojRacuna, string iznos, string datumIzdavanja, string userId)
        {
            if (iznos == null)
            {
                return new(new { success = false, Message = "iznos ne smije biti prazan!" });
            }

            if ((await _c.UnitOfWork.BillsElektra.TempList(userId)).Count() >= 500)
            {
                return new(new { success = false, Message = "U tablici ne može biti više od 500 računa!" });
            }

            double _iznos = double.Parse(iznos);
            DateTime? _datumIzdavanja = datumIzdavanja is not null ? DateTime.Parse(datumIzdavanja) : null;

            RacunElektra re = new()
            {
                BrojRacuna = brojRacuna,
                Iznos = _iznos,
                DatumIzdavanja = _datumIzdavanja,
                CreatedByUserId = userId,
                IsItTemp = true,
            };

            if (re.BrojRacuna.Length >= 10)
            {
                re.ElektraKupac = await _c.UnitOfWork.ElektraCustomer.FindExact(e => e.UgovorniRacun == long.Parse(re.BrojRacuna.Substring(0, 10)));
                if (re.ElektraKupac == null)
                {
                    re.ElektraKupac = await _c.UnitOfWork.ElektraCustomer.FindExact(e => e.Id == 2002);
                }
            }
            else
            {
                re.ElektraKupac = await _c.UnitOfWork.ElektraCustomer.FindExact(e => e.Id == 2002);
            }

            IEnumerable<RacunElektra> tempRacuni = (await _c.UnitOfWork.BillsElektra.TempList(userId)).Append(re);

            int rbr = 1;
            foreach (RacunElektra e in tempRacuni)
            {
                e.RedniBroj = rbr++;
            }
            _c.UnitOfWork.BillsElektra.Add(re);
            return await _c.Service.TrySave(false);
        }

        public async Task<int> CheckTempTableForBillsWithousElektraCustomer(string userId)
        {
            IEnumerable<RacunElektra> list = await _c.UnitOfWork.BillsElektra.TempList(userId);
            return list.Count(e => e.ElektraKupacId == 2002);
        }

        public async Task<JsonResult> RefreshCustomers(string userId)
        {
            IEnumerable<RacunElektra> tempRacuni = await _c.UnitOfWork.BillsElektra.TempList(userId);

            if (tempRacuni.Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }

            foreach (RacunElektra e in tempRacuni)
            {
                if (e.ElektraKupacId == 2002 && e.BrojRacuna.Length >= 10)
                {
                    e.ElektraKupac = await _c.UnitOfWork.BillsElektra.GetKupacByUgovorniRacun(long.Parse(e.BrojRacuna[..10]));
                    if (e.ElektraKupac == null)
                    {
                        e.ElektraKupac = await _c.UnitOfWork.ElektraCustomer.FindExact(e => e.Id == 2002);
                    }
                }
            }

            await BillsElektraNotesBuild(userId, tempRacuni);
            return new(new { success = true, Message = "Podaci su osvježeni" });
        }

        private async Task BillsElektraNotesBuild(string userId, IEnumerable<RacunElektra> list)
        {
            foreach (RacunElektra e in list)
            {
                if (e.ElektraKupacId == 2002)
                {
                    e.Napomena = "kupac ne postoji";
                }
                else
                {
                    IEnumerable<RacunElektra> bills = await _c.UnitOfWork.BillsElektra.GetRacuniForCustomer((int)e.ElektraKupacId);
                    e.Napomena = await _c.BillsCheckService.CheckIfExistsInPayed(e.BrojRacuna, bills);
                }

                if (e.Napomena is null)
                {
                    IEnumerable<RacunElektra> tempBills = await _c.UnitOfWork.BillsElektra.Find(item => item.IsItTemp == true && item.CreatedByUserId == userId && item.ElektraKupacId == e.ElektraKupacId);
                    e.Napomena = await _c.BillsCheckService.CheckIfExistsInTemp(e.BrojRacuna, tempBills);
                }

                _ = await _c.UnitOfWork.BillsElektra.Update(e);
            }
        }
    }
}