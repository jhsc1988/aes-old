using aes.CommonDependecies.ICommonDependencies;
using aes.Models.Racuni.Elektra;
using aes.Services.RacuniServices.Elektra.RacuniElektraRate.Is;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.Elektra.RacuniElektraRate
{
    public class RacuniElektraRateTempCreateService : IRacuniElektraRateTempCreateService
    {
        private readonly IRacuniCommonDependecies _c;

        public RacuniElektraRateTempCreateService(IRacuniCommonDependecies c)
        {
            _c = c;
        }
        public async Task<JsonResult> AddNewTemp(string brojRacuna, string iznos, string razdoblje, string userId)
        {
            if (iznos == null)
            {
                return new(new { success = false, Message = "iznos ne smije biti prazan!" });

            }

            if ((await _c.UnitOfWork.RacuniElektraRate.TempList(userId)).Count() >= 500)
            {
                return new(new { success = false, Message = "U tablici ne može biti više od 500 računa!" });
            }

            decimal _iznos = decimal.Parse(iznos);
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
                re.ElektraKupac = await _c.UnitOfWork.ElektraKupac.FindExact(e => e.UgovorniRacun == long.Parse(re.BrojRacuna.Substring(0, 10)));
                re.ElektraKupac ??= await _c.UnitOfWork.ElektraKupac.FindExact(e => e.Id == 2002);
            }
            else
            {
                re.ElektraKupac = await _c.UnitOfWork.ElektraKupac.FindExact(e => e.Id == 2002);
            }

            IEnumerable<RacunElektraRate> RacunElektraRateList = (await _c.UnitOfWork.RacuniElektraRate.TempList(userId)).Append(re);

            int rbr = 1;
            foreach (RacunElektraRate e in RacunElektraRateList)
            {
                e.RedniBroj = rbr++;
            }

            _c.UnitOfWork.RacuniElektraRate.Add(re);

            return await _c.Service.TrySave(false);
        }

        public async Task<int> CheckTempTableForRacuniWithousElectraCustomer(string userId)
        {
            IEnumerable<RacunElektraRate> list = await _c.UnitOfWork.RacuniElektraRate.Find(e => e.CreatedByUserId.Equals(userId) && e.IsItTemp == true);
            return list.Count(e => e.ElektraKupacId == 2002);
        }

        public async Task<JsonResult> RefreshCustomers(string userId)
        {
            IEnumerable<RacunElektraRate> list = await _c.UnitOfWork.RacuniElektraRate.TempList(userId);

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
                    e.ElektraKupac = await _c.UnitOfWork.RacuniElektraRate.GetKupacByUgovorniRacun(long.Parse(e.BrojRacuna[..10]));
                    e.ElektraKupac ??= await _c.UnitOfWork.ElektraKupac.FindExact(e => e.Id == 2002);
                }
            }
            await RacuniElektraNotesBuild(userId, list);
            return new(new { success = true, Message = "Podaci su osvježeni" });
        }

        private async Task RacuniElektraNotesBuild(string userId, IEnumerable<RacunElektraRate> list)
        {
            foreach (RacunElektraRate e in list)
            {
                if (e.ElektraKupacId == 2002)
                {
                    e.Napomena = "kupac ne postoji";
                }
                else
                {
                    IEnumerable<RacunElektraRate> Racuni = await _c.UnitOfWork.RacuniElektraRate.GetRacuniForCustomer((int)e.ElektraKupacId);
                    e.Napomena = _c.RacuniCheckService.CheckIfExistsInPayed(e.BrojRacuna, Racuni);
                }

                if (e.Napomena is null)
                {
                    IEnumerable<RacunElektraRate> tempRacuni = await _c.UnitOfWork.RacuniElektraRate.Find(item => item.IsItTemp == true && item.CreatedByUserId == userId && item.ElektraKupacId == e.ElektraKupacId);
                    e.Napomena = _c.RacuniCheckService.CheckIfExistsInTemp(e.BrojRacuna, tempRacuni);
                }

                _ = await _c.UnitOfWork.RacuniElektraRate.Update(e);
            }
        }
    }
}
