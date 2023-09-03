using aes.CommonDependecies.ICommonDependencies;
using aes.Models.Datatables;
using aes.Services.RacuniServices.IRacuniService;
using aes.Services.RacuniServices.IServices;
using aes.UnitOfWork;

namespace aes.CommonDependecies
{
    public class RacuniCommonDependecies : CommonDependencies, IRacuniCommonDependecies
    {
        public IRacuniInlineEditorService RacuniInlineEditorService { get; }
        public IRacuniTempEditorService RacuniTempEditorService { get; }
        public IRacuniCheckService RacuniCheckService { get; }
        public IService Service { get; }

        public RacuniCommonDependecies(IDatatablesGenerator datatablesGenerator, IDatatablesSearch datatablesSearch, IUnitOfWork unitOfWork,
            IRacuniInlineEditorService racuniInlineEditorService, IRacuniTempEditorService racuniTempEditorService, IService service,
            IRacuniCheckService racuniCheckService)
            : base(datatablesGenerator, datatablesSearch, unitOfWork)
        {
            RacuniInlineEditorService = racuniInlineEditorService;
            RacuniTempEditorService = racuniTempEditorService;
            RacuniCheckService = racuniCheckService;
            Service = service;
        }
    }
}
