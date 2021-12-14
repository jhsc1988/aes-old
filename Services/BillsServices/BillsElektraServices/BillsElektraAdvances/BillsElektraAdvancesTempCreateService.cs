using aes.CommonDependecies;
using aes.Models.Racuni;
using aes.Services.BillsServices.BillsElektraServices.BillsElektraAdvances.Is;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.BillsElektraServices.BillsElektraAdvances
{
    public class BillsElektraAdvancesTempCreateService : IBillsElektraAdvancesTempCreateService
    {
        private readonly IBillsCommonDependecies _c;

        public BillsElektraAdvancesTempCreateService(IBillsCommonDependecies c)
        {
            _c = c;
        }
        public async Task<JsonResult> AddNewTemp(string brojRacuna, string iznos, string razdoblje, string userId)
        {
            if (iznos == null)
            {
                return new(new { success = false, Message = "iznos ne smije biti prazan!" });

            }

            if ((await _c.UnitOfWork.BillsElektraAdvances.TempList(userId)).Count() >= 500)
            {
                return new(new { success = false, Message = "U tablici ne može biti više od 500 računa!" });
            }

            double _iznos = double.Parse(iznos);
            DateTime? _razdoblje = razdoblje is not null ? DateTime.Parse(razdoblje) : null;

            RacunElektraRate re = new()
            {
                BrojRacuna = brojRacuna,
                Iznos = _iznos,
                DatumIzdavanja = _razdoblje,
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

            IEnumerable<RacunElektraRate> RacunElektraRateList = (await _c.UnitOfWork.BillsElektraAdvances.TempList(userId)).Append(re);

            int rbr = 1;
            foreach (RacunElektraRate e in RacunElektraRateList)
            {
                e.RedniBroj = rbr++;
            }

            _c.UnitOfWork.BillsElektraAdvances.Add(re);

            return await _c.Service.TrySave(false);
        }

        public async Task<int> CheckTempTableForBillsWithousElectraCustomer(string userId)
        {
            IEnumerable<RacunElektraRate> list = await _c.UnitOfWork.BillsElektraAdvances.Find(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true);
            return list.Count(e => e.ElektraKupacId == 2002);
        }

        public async Task<JsonResult> RefreshCustomers(string userId)
        {
            IEnumerable<RacunElektraRate> list = await _c.UnitOfWork.BillsElektraAdvances.TempList(userId);

            if (list.Count() is 0)
            {
                return new(new
                {
                    success = false,
                    Message = "U tablici nema podataka"
                });
            }

            foreach (RacunElektraRate e in list)
            {
                if (e.ElektraKupacId == 2002 && e.BrojRacuna.Length >= 10)
                {
                    e.ElektraKupac = await _c.UnitOfWork.BillsElektraAdvances.GetKupacByUgovorniRacun(long.Parse(e.BrojRacuna[..10]));
                    if (e.ElektraKupac == null)
                    {
                        e.ElektraKupac = await _c.UnitOfWork.ElektraCustomer.FindExact(e => e.Id == 2002);
                    }
                }
            }
            await BillsElektraNotesBuild(userId, list);
            return new(new { success = true, Message = "Podaci su osvježeni" });
        }

        private async Task BillsElektraNotesBuild(string userId, IEnumerable<RacunElektraRate> list)
        {
            foreach (RacunElektraRate e in list)
            {
                if (e.ElektraKupacId == 2002)
                {
                    e.Napomena = "kupac ne postoji";
                }
                else
                {
                    IEnumerable<RacunElektraRate> bills = await _c.UnitOfWork.BillsElektraAdvances.GetRacuniForCustomer((int)e.ElektraKupacId);
                    e.Napomena = await _c.BillsCheckService.CheckIfExistsInPayed(e.BrojRacuna, bills);
                }

                if (e.Napomena is null)
                {
                    IEnumerable<RacunElektraRate> tempBills = await _c.UnitOfWork.BillsElektraAdvances.Find(item => item.IsItTemp == true && item.CreatedByUserId == userId && item.ElektraKupacId == e.ElektraKupacId);
                    e.Napomena = await _c.BillsCheckService.CheckIfExistsInTemp(e.BrojRacuna, tempBills);
                }

                _ = await _c.UnitOfWork.BillsElektraAdvances.Update(e);
            }
        }
    }
}
