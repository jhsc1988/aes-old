using aes.Services.RacuniServices;
using aes.Services.RacuniServices.IRacuniService;

namespace aes.CommonDependecies
{
    public interface IRacuniCommonDependecies : ICommonDependencies
    {
        IRacuniInlineEditorService RacuniInlineEditorService { get; }
        IRacuniTempEditorService RacuniTempEditorService { get; }
        IService Service { get; }
        IRacuniCheckService RacuniCheckService { get; }
    }
}