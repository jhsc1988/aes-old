using aes.Models.Racuni;
using aes.Services.RacuniServices.IRacuniService;
using aes.Services.RacuniServices.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices
{
    public class RacuniTempEditorService : IRacuniTempEditorService
    {
        private readonly IService _service;

        public RacuniTempEditorService(IService service)
        {
            _service = service;
        }

        public async Task<JsonResult> SaveToDb<T>(IEnumerable<Racun> RacunListToSave, string _dopisId) where T : Racun
        {
            List<Racun> racunList = new();
            int dopisId = int.Parse(_dopisId);
            if (dopisId is 0)
            {
                return new(new { success = false, Message = "Nije odabran dopis!" });
            }

            racunList.AddRange(RacunListToSave.ToList());

            foreach (Racun e in racunList)
            {
                e.DopisId = dopisId;
                e.IsItTemp = null;
                e.Napomena = null;
            }

            return await _service.TrySave(false);
        }
    }
}
