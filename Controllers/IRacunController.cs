using aes.Data;
using aes.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Controllers
{
    interface IRacunController
    {

        /// <summary>
        /// Gets user id
        /// </summary>
        /// <returns>string</returns>
        public string GetUid();

        /// <summary>
        /// Gets params from Datatables which was requested by Datatables AJAX POST method
        /// </summary>
        public void GetDatatablesParamas();

        /// <summary>
        /// Gets list of all Racuni
        /// </summary>
        /// <param name="klasa"></param>
        /// <param name="urbroj"></param>
        /// <returns>async Task<IActionResult> (JSON)</returns>
        public Task<IActionResult> GetList(string klasa, string urbroj);

        /// <summary>
        /// Gets list of all Racuni with userID and IsItTemp = true
        /// </summary>
        /// <returns>Task<IActionResult></returns>
        public Task<IActionResult> GetListCreate();

        /// <summary>
        /// Gets predmeti for filtered data
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetPredmetiDataForFilter();

        /// <summary>
        /// Gets dopisi for predmet for filtered data
        /// </summary>
        /// <param name="predmetId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetDopisiDataForFilter(int predmetId);

        /// <summary>
        /// Gets list of predmeti for dropdown on Create page
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPredmetiCreate();

        /// <summary>
        /// Gets list of dopisi for predmet for dropdown on Create page
        /// </summary>
        /// <param name="predmetId">Id of Predmet</param>
        /// <returns>JsonResult</returns>
        public JsonResult GetDopisiCreate(int predmetId);

        /// <summary>
        /// Gets list of Kupci for notification (info) builder
        /// </summary>
        /// <returns>string</returns>
        public string GetKupci();

        /// <summary>
        /// Db update on inline edit
        /// </summary>
        /// <param name="id">Id of Racun</param>
        /// <param name="updatedColumn">Column which was updated</param>
        /// <param name="x">Changed text variable</param>
        /// <returns></returns>
        public JsonResult UpdateDbForInline(string id, string updatedColumn, string x);

        /// <summary>
        /// Adds new row to RacunElektraTemp
        /// </summary>
        /// <param name="brojRacuna">Broj računa</param>
        /// <param name="iznos">Iznos računa</param>
        /// <param name="date">Datum izdavanja</param>
        /// <param name="__guid">Guid // TODO: use UserID instead</param>
        /// <returns></returns>
        public JsonResult AddNewTemp(string brojRacuna, string iznos, string date, string dopisId);

        /// <summary>
        /// Moves from RacunElektraTemp to RacuniElektra table
        /// </summary>
        /// <param name="_dopisid">Id of Dopis</param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveToDB(string _dopisid);

        /// <summary>
        /// Remove Row from RacunElektraTemp
        /// </summary>
        /// <param name="racunId">Id of RacunElektra</param>
        /// <returns>async Task<IActionResult></returns>
        public JsonResult RemoveRow(string racunId);

        /// <summary>
        /// Used for Emptying entries in Create page
        /// Ment to use with static RemoveAllFromDb(GetUid(), _context)
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult RemoveAllFromDb();
    }
}
