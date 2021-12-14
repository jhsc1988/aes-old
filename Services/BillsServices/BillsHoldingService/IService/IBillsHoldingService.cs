using aes.Models.Racuni;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Services.BillsServices.BillsHoldingService.IService
{
    public interface IBillsHoldingService : IBillsService.IBillService
    {
        Task<IEnumerable<RacunHolding>> GetCreateBills(string userId);
        Task<IEnumerable<RacunHolding>> GetList(int predmetIdAsInt, int dopisIdAsInt);
    }
}