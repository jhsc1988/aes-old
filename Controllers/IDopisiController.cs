using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Controllers
{
    internal interface IDopisiController
    {
        /// <summary>
        /// Gets params from Datatables which was requested by Datatables AJAX POST method
        /// </summary>
        public void GetDatatablesParamas();

        /// <summary>
        /// Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze
        /// </summary>
        /// <returns>Vraća dopisa u JSON obliku za server side processing</returns>
        public Task<IActionResult> GetList(int predmetId);

        /// <summary>
        /// Saves changes to Db
        /// </summary>
        /// <param name="predmetId">ID predmeta</param>
        /// <param name="urbroj">Urbroj</param>
        /// <param name="datumDopisa">datum dopisa</param>
        /// <returns>rezultat u JSON</returns>
        public JsonResult SaveToDB(string predmetId, string urbroj, string datumDopisa);

        /// <summary>
        /// Saves changes to db
        /// </summary>
        /// <returns>rezultat u JSON</returns>
        public JsonResult TrySave();
    }
}
