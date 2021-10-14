using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Controllers
{
    internal interface IRacunController
    {

        /// <summary>
        /// Gets user id
        /// </summary>
        /// <returns>string</returns>
        public string GetUid();

        /// <summary>
        /// Gets predmeti
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetPredmetiDataForFilter();

        /// <summary>
        /// Gets dopisi
        /// </summary>
        /// <param name="predmetId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetDopisiDataForFilter(int predmetId);

        /// <summary>
        /// Gets list of predmeti for dropdown on Create page
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetPredmetiCreate();

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
        /// <returns>JsonResult</returns>
        public JsonResult UpdateDbForInline(string id, string updatedColumn, string x);

        /// <summary>
        /// Saves Racun as IsItTemp = null
        /// </summary>
        /// <param name="_dopisid">Id of Dopis</param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveToDB(string _dopisid);

        /// <summary>
        /// Remove Row from RacunElektraTemp (on Create page)
        /// </summary>
        /// <param name="racunId">Id of RacunElektra</param>
        /// <returns>async Task<IActionResult></returns>
        public JsonResult RemoveRow(string racunId);

        /// <summary>
        /// Used for Emptying entries in Create page
        /// Ment to use with RemoveAllFromDb(GetUid(), DbSet<T>, _context)
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult RemoveAllFromDb();
        JsonResult GetList(bool isFIltered, string klasa, string urbroj);
    }
}
