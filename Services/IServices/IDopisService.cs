using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services.IServices
{
    public interface IDopisiervice
    {
        Task<JsonResult> GetList(int predmetId, HttpRequest Request);
        Task<JsonResult> SaveToDB(string predmetId, string urbroj, string datumDopisa);
    }
}