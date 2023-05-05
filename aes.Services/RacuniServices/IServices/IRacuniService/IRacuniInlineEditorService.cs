using aes.Models.Racuni;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.RacuniServices.IRacuniService
{
    public interface IRacuniInlineEditorService
    {
        Task<JsonResult> UpdateDbForInline<T>(Racun RacunToUpdate, string updatedColumn, string x) where T : Racun;
    }
}