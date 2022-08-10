using aes.CommonDependecies;
using aes.Models;
using aes.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Services
{
    public class Predmetiervice : IPredmetiervice
    {
        private readonly ICommonDependencies _c;
        public Predmetiervice(ICommonDependencies c)
        {
            _c = c;
        }

        public async Task<JsonResult> SaveToDB(string klasa, string naziv)
        {
            Predmet pTemp = new();

            pTemp.Klasa = klasa;
            pTemp.Naziv = naziv;

            _c.UnitOfWork.Predmet.Add(pTemp);

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
