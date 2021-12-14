using aes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aes.Repository.IRepository
{
    public interface ILetterRepository : IRepository<Dopis>
    {
        Task<IEnumerable<Dopis>> GetLettersForCaseFile(int predmetId);
        Task<IEnumerable<Dopis>> GetOnlyEmptyLetters(int predmetId);
        Task<Dopis> IncludeCaseFile(Dopis letter);
    }
}