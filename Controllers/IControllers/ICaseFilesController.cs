using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Controllers.IControllers
{
    internal interface ICaseFilesController
    {

        Task<JsonResult> GetList();
        Task<JsonResult> SaveToDB(string klasa, string naziv);
    }
}
