using aes.Models.Racuni;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.IRacuniService
{
    public interface IRacuniTempEditorService
    {
        Task<JsonResult> SaveToDb<T>(IEnumerable<Racun> RacunListToSave, string _dopisId) where T : Racun;
    }
}