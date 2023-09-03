using aes.Services.RacuniServices.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using aes.UnitOfWork;

namespace aes.Services.RacuniServices
{
    public class Service : IService
    {
        private readonly IUnitOfWork _unitOfWork;
        public Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delete">true for delete, false for save</param>
        /// <returns></returns>
        public async Task<JsonResult> TrySave(bool delete)
        {
            try
            {
                _ = await _unitOfWork.Complete();

                return delete
                    ? (new(new { success = true, Message = "Obrisano" }))
                    : (new(new { success = true, Message = "Spremljeno" }));
            }
            catch (DbUpdateException)
            {
                return new(new { success = false, Message = "Greška" });
            }
        }

        public string GetUid(ClaimsPrincipal User)
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
