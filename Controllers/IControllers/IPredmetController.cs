using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Controllers
{
    internal interface IPredmetController
    {
        //public void GetDatatablesParamas();

        ///// <summary>
        ///// Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze
        ///// </summary>
        ///// <returns>Vraća listu predmeta u JSON obliku za server side processing</returns>
        public Task<IActionResult> GetList();

        /// <summary>
        /// Adds new predmet to context
        /// </summary>
        /// <param name="klasa"> klasa predmeta</param>
        /// <param name="naziv">naziv predmeta</param>
        /// <returns>rezultat spremanja u JSON</returns>
        public JsonResult SaveToDB(string klasa, string naziv);

        /// <summary>
        /// Saves changes to db
        /// </summary>
        /// <returns>rezultat u JSON</returns>
        public JsonResult TrySave();
    }
}
