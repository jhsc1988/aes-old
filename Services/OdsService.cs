using aes.CommonDependecies;
using aes.Models;
using aes.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services
{
    public class OdsService : IOdsService
    {
        private readonly ICommonDependencies _c;

        public OdsService(ICommonDependencies c)
        {
            _c = c;
        }

        public async Task<JsonResult> GetStanData(string sid)
        {
            int idInt;
            if (sid is not null)
            {
                idInt = int.Parse(sid);
            }
            else
            {
                return new JsonResult(
                    new
                    {
                        success = false,
                        Message = "Greška, prazan id"
                    });
            }

            Stan stan = await _c.UnitOfWork.Stan.Get(idInt);
            return new JsonResult(stan);
        }

        public async Task<JsonResult> GetStanDataForOmm(string OdsId)
        {
            int OdsIdInt;
            if (OdsId is not null)
            {
                OdsIdInt = int.Parse(OdsId);
            }
            else
            {
                return new JsonResult(
                    new
                    {
                        success = false,
                        Message = "Greška, prazan id"
                    });
            }

            Ods ods = await _c.UnitOfWork.Ods.IncludeAppartment(await _c.UnitOfWork.Ods.Get(OdsIdInt));
            return new JsonResult(ods);
        }
    }
}
