using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Controllers.IControllers
{
    internal interface IPredmetiController
    {

        Task<JsonResult> GetList();
        Task<JsonResult> SaveToDB(string klasa, string naziv);
    }
}
