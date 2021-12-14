using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Controllers.IControllers
{
    internal interface ILetterController
    {
        Task<JsonResult> GetList(int predmetId);
        Task<JsonResult> SaveToDB(string predmetId, string urbroj, string datumDopisa);
    }
}
