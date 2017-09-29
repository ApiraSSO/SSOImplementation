using System.Threading.Tasks;

namespace Kernel.Federation.MetaData
{
    public interface IFederationMetadataDispatcher
    {
        Task Dispatch(DispatcherContext dispatcherContext);
    }
}