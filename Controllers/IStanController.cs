using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace aes.Controllers
{
    internal interface IStanController
    {
        /// <summary>
        /// Gets params from Datatables which was requested by Datatables AJAX POST method
        /// </summary>
        public void GetDatatablesParamas();

        /// <summary>
        /// Server side processing - učitavanje, filtriranje, paging, sortiranje podataka
        /// </summary>
        /// <returns>Vraća filtriranu listu stanova u JSON obliku za server side processing</returns>
        public Task<IActionResult> GetList();

        /// <summary>
        /// Server side processing - učitavanje, filtriranje, paging, sortiranje podataka iz baze za dodavanje novog OMM-ODS
        /// </summary>
        /// <returns>Vraća filtriranu listu stanova (za koje ne postoje omm HEP-ODS-a) u JSON obliku za server side processing</returns>
        public Task<IActionResult> GetListFiltered();

        /// <summary>
        /// Racuni elektra za stan - koristi se u Details.cshtml
        /// </summary>
        /// <param name="param"></param>
        /// <returns>JSON lista računa</returns>
        public Task<IActionResult> GetRacuniForStan(int param);

        /// <summary>
        /// Racuni elektra - rate za stan - koristi se u Details.cshtml
        /// </summary>
        /// <param name="param"></param>
        /// <returns>JSON lista računa</returns>
        public Task<IActionResult> GetRacuniRateForStan(int param);

        /// <summary>
        /// Racuni holding za stan - koristi se u Details.cshtml
        /// </summary>
        /// <param name="param"></param>
        /// <returns>JSON lista računa</returns>
        public Task<IActionResult> GetHoldingRacuniForStan(int param);

        /// <summary>
        /// Racuni elektra - izvrsenje usluge za stan - koristi se u Details.cshtml
        /// </summary>
        /// <param name="param"></param>
        /// <returns>JSON lista računa</returns>
        public Task<IActionResult> GetRacuniElektraIzvrsenjeForStan(int param);
    }
}
