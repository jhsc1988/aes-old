using aes.Models.Datatables;
using aes.UnitOfWork;

namespace aes.CommonDependecies
{
    public class CommonDependencies : ICommonDependencies.ICommonDependencies
    {
        public IDatatablesGenerator DatatablesGenerator { get; }
        public IDatatablesSearch DatatablesSearch { get; }
        public IUnitOfWork UnitOfWork { get; }

        public CommonDependencies(IDatatablesGenerator datatablesGenerator,
            IDatatablesSearch datatablesSearch, IUnitOfWork unitOfWork)
        {
            DatatablesGenerator = datatablesGenerator;
            DatatablesSearch = datatablesSearch;
            UnitOfWork = unitOfWork;
        }
    }
}
