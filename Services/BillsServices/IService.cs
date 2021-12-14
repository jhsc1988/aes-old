using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace aes.Services.BillsServices
{
    public interface IService
    {
        string GetUid(ClaimsPrincipal User);
        Task<JsonResult> TrySave(bool delete);
    }
}