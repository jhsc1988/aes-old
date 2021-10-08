using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Controllers
{
    internal interface IOdsController
    {
        public void GetDatatablesParamas();

        /// <summary>
        /// Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze
        /// </summary>
        /// <returns>Vraća listu obračunskih mjernih mjesta za HEP - ODS u JSON obliku za server side processing</returns>
        public Task<IActionResult> GetList();

        /// <summary>
        /// Get kupci for notification builder
        /// </summary>
        /// <param name="sid">Stan ID - primarni ključ</param>
        /// <returns>rezultat u JSON - vraća stan za id</returns>
        public JsonResult GetStanData(string sid);
    }
}
