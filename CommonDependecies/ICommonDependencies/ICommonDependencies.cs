using aes.Models.Datatables;
using aes.UnitOfWork;

namespace aes.CommonDependecies.ICommonDependencies
{
    public interface ICommonDependencies
    {
        IDatatablesGenerator DatatablesGenerator { get; }
        IDatatablesSearch DatatablesSearch { get; }
        IUnitOfWork UnitOfWork { get; }
    }
}