using aes.Data;
using aes.Models.Workshop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace aes.Models
{
    public interface IRacunWorkshop : IWorkshop
    {
        /// <summary>
        /// Provjerava postoji li isti račun u tablici - koristi se za napomenu
        /// </summary>
        /// <param name="brojRacuna"></param>
        /// <param name="racunList"></param>
        /// <returns>string - Error msg or null</returns>
        string CheckIfExists(string brojRacuna, List<Racun> racunList);

        /// <summary>
        /// Provjerava postoji li račun u plaćenim računima - koristi se za napomenu
        /// </summary>
        /// <param name="brojRacuna"></param>
        /// <param name="racunList"></param>
        /// <returns>string - Error msg or null</returns>
        string CheckIfExistsInPayed(string brojRacuna, List<Racun> racunList);

        /// <summary>
        /// Removes row from table - kada račun nije za plaćanje
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="racunId"></param>
        /// <param name="_modelcontext"></param>
        /// <param name="_context"></param>
        /// <returns>JsonResult - result of TrySave(ApplicationDbContext, bool)</returns>
        JsonResult RemoveRow<T>(string racunId, DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun;

        /// <summary>
        /// Validacija prilikom pohrane - osigurava ispravnost unosa i parsira stringove u num
        /// </summary>
        /// <param name="brojRacuna"></param>
        /// <param name="iznos"></param>
        /// <param name="date">datum izdavanja u string-u</param>
        /// <param name="dopisId"></param>
        /// <param name="msg"></param>
        /// <param name="_iznos"></param>
        /// <param name="_dopisId"></param>
        /// <param name="datumIzdavanja">datum izdavanja kao DateTime</param>
        /// <returns>bool - is it valid or not</returns>
        bool Validate(string brojRacuna, string iznos, string date, string dopisId, out string msg, out double _iznos, out int _dopisId, out DateTime? datumIzdavanja);

        /// <summary>
        /// Unos podataka za inline editor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="racunId"></param>
        /// <param name="updatedColumn"></param>
        /// <param name="x"></param>
        /// <param name="_modelcontext"></param>
        /// <param name="_context"></param>
        /// <returns>JsonResult - vraća rezultat od TrySave(ApplicationDbContext, bool)</returns>
        JsonResult UpdateDbForInline<T>(string racunId, string updatedColumn, string x, DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun;

        /// <summary>
        /// Validacija i spremanje u bazu
        /// </summary>
        /// <typeparam name="T">Racun</typeparam>
        /// <param name="userId"></param>
        /// <param name="_dopisId"></param>
        /// <param name="_modelcontext"></param>
        /// <param name="_context"></param>
        /// <returns>JsonResult - vraća rezultat od TrySave(ApplicationDbContext, bool)</returns>
        JsonResult SaveToDb<T>(string userId, string _dopisId, DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun;

        /// <summary>
        /// Briše cijelu temp tablicu (svi koji su IsItTemp == true)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userId"></param>
        /// <param name="_modelcontext"></param>
        /// <param name="_context"></param>
        /// <returns>JsonResult - vraća rezultat od TrySave(ApplicationDbContext, bool)</returns>
        JsonResult RemoveAllFromDbTemp<T>(string userId, DbSet<T> _modelcontext, ApplicationDbContext _context) where T : Racun;

        /// <summary>
        /// Gets user id
        /// </summary>
        /// <param name="User"></param>
        /// <returns>user id string</returns>
        string GetUid(ClaimsPrincipal User);
    }
}
