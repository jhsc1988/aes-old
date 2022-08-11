using aes.Services.RacuniServices.IRacuniService;
using aes.Services.RacuniServices.IServices;

namespace aes.CommonDependecies.ICommonDependencies
{
    public interface IRacuniCommonDependecies : ICommonDependencies
    {
        IRacuniInlineEditorService RacuniInlineEditorService { get; }
        IRacuniTempEditorService RacuniTempEditorService { get; }
        IService Service { get; }
        IRacuniCheckService RacuniCheckService { get; }
    }
}