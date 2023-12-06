using aes.CommonDependecies.ICommonDependencies;
using aes.Models;
using aes.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace aes.Services
{
    public class PredmetiService : IPredmetiervice
    {
        private readonly ICommonDependencies _c;

        public PredmetiService(ICommonDependencies c)
        {
            _c = c;
        }

        public async Task<JsonResult> SaveToDB(string klasa, string naziv)
        {
            Predmet pTemp = new()
            {
                Klasa = klasa,
                Naziv = naziv
            };

            await _c.UnitOfWork.Predmet.Add(pTemp);

            int numOfSaved = await _c.UnitOfWork.Complete();
            return numOfSaved == 0
                ? (new(new
                {
                    success = false,
                    Message = "Error"
                }))
                : (new(new
                {
                    success = true,
                    Message = numOfSaved
                }));
        }
    }
}