using aes.Models.Racuni;
using aes.Services.BillsServices.IBillsService;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Services.BillsServices
{
    public class BillsTempEditorService : IBillsTempEditorService
    {
        private readonly IService _service;

        public BillsTempEditorService(IService service)
        {
            _service = service;
        }

        public async Task<JsonResult> SaveToDb<T>(IEnumerable<Racun> billListToSave, string _dopisId) where T : Racun
        {
            List<Racun> racunList = new();
            int dopisId = int.Parse(_dopisId);
            if (dopisId is 0)
            {
                return new(new { success = false, Message = "Nije odabran dopis!" });
            }

            racunList.AddRange(billListToSave.ToList());

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
