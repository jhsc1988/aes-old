using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Controllers.IControllers
{
    internal interface IBillsController
    {
        Task<JsonResult> UpdateDbForInline(string id, string updatedColumn, string x);
        Task<JsonResult> SaveToDB(string _dopisid);
        Task<JsonResult> RemoveRow(string racunId);
        Task<JsonResult> RemoveAllFromDb();
        Task<JsonResult> GetLettersDataForCreate(int predmetId);
        Task<JsonResult> GetList(bool isFilteredForIndex, string klasa, string urbroj);
        Task<JsonResult> GetCaseFilesDataForFilter();
        Task<JsonResult> GetLettersDataForFilter(int predmetId);
        Task<JsonResult> GetCaseFilesForCreate();
        Task<string> GetCustomers();
    }
}
