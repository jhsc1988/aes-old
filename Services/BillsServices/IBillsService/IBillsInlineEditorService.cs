using aes.Models.Racuni;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.IBillsService
{
    public interface IBillsInlineEditorService
    {
        Task<JsonResult> UpdateDbForInline<T>(Racun billToUpdate, string updatedColumn, string x) where T : Racun;
    }
}