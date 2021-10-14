using Microsoft.AspNetCore.Mvc;

namespace aes.Controllers
{
    internal interface IDopisiController
    {
        /// <summary>
        /// Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze
        /// </summary>
        /// <returns>Vraća dopisa u JSON obliku za server side processing</returns>
        public JsonResult GetList(int predmetId);

        /// <summary>
        /// Adds new dopis to context
        /// </summary>
        /// <param name="predmetId">ID predmeta</param>
        /// <param name="urbroj">Urbroj</param>
        /// <param name="datumDopisa">datum dopisa</param>
        /// <returns>rezultat spremanja u JSON</returns>
        public JsonResult SaveToDB(string predmetId, string urbroj, string datumDopisa);
    }
}
