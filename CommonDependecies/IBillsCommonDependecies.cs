using aes.Services.BillsServices;
using aes.Services.BillsServices.IBillsService;

namespace aes.CommonDependecies
{
    public interface IBillsCommonDependecies : ICommonDependencies
    {
        IBillsInlineEditorService BillsInlineEditorService { get; }
        IBillsTempEditorService BillsTempEditorService { get; }
        IService Service { get; }
        IBillsCheckService BillsCheckService { get; }
        IBillsValidationService BillsValidationService { get; }
    }
}