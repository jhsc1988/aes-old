using Microsoft.AspNetCore.Mvc;

namespace aes.Controllers.IControllers
{
    internal interface IPredmetiController
    {

        Task<JsonResult> GetList();
        Task<JsonResult> SaveToDB(string klasa, string naziv);
    }
}
