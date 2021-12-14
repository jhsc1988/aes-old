using aes.Models.Racuni;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.IBillsService
{
    public interface IBillsTempEditorService
    {
        Task<JsonResult> SaveToDb<T>(IEnumerable<Racun> billListToSave, string _dopisId) where T : Racun;
    }
}