using aes.Models.Datatables;
using aes.Repository.UnitOfWork;
using aes.Services.BillsServices;
using aes.Services.BillsServices.IBillsService;

namespace aes.CommonDependecies
{
    public class BillsCommonDependecies : CommonDependencies, IBillsCommonDependecies
    {
        public IBillsInlineEditorService BillsInlineEditorService { get; }
        public IBillsTempEditorService BillsTempEditorService { get; }
        public IBillsCheckService BillsCheckService { get; }
        public IService Service { get; }

        public BillsCommonDependecies(IDatatablesGenerator datatablesGenerator, IDatatablesSearch datatablesSearch, IUnitOfWork unitOfWork,
            IBillsInlineEditorService billsInlineEditorService, IBillsTempEditorService billsTempEditorService, IService service,
            IBillsCheckService billsCheckService)
            : base(datatablesGenerator, datatablesSearch, unitOfWork)
        {
            BillsInlineEditorService = billsInlineEditorService;
            BillsTempEditorService = billsTempEditorService;
            BillsCheckService = billsCheckService;
            Service = service;
        }
    }
}
