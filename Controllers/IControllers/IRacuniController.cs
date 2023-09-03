using Microsoft.AspNetCore.Mvc;

namespace aes.Controllers.IControllers
{
    internal interface IRacuniController
    {
        Task<JsonResult> UpdateDbForInline(string id, string updatedColumn, string x);
        Task<JsonResult> SaveToDB(string _dopisid);
        Task<JsonResult> RemoveRow(string racunId);
        Task<JsonResult> RemoveAllFromDb();
        Task<JsonResult> GetDopisiDataForCreate(int predmetId);
        Task<JsonResult> GetList(bool isFilteredForIndex, string klasa, string urbroj);
        Task<JsonResult> GetPredmetiDataForFilter();
        Task<JsonResult> GetDopisiDataForFilter(int predmetId);
        Task<JsonResult> GetPredmetiForCreate();
        Task<string> GetCustomers();
    }
}
