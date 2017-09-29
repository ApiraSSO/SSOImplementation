using System.Threading.Tasks;
using Kernel.DependancyResolver;
using Kernel.Federation.MetaData;

namespace WsFederationMetadataDispatcher
{
    internal class FederationMetadataDispatcher : IFederationMetadataDispatcher
    {
        private readonly IDependencyResolver _dependencyResolver;
        public FederationMetadataDispatcher(IDependencyResolver dependencyResolver)
        {
            this._dependencyResolver = dependencyResolver;
        }
        public async Task Dispatch(DispatcherContext dispatcherContext)
        {
            var writers = this._dependencyResolver.ResolveAll<IFederationMetadataWriter>();
            foreach(var writer in writers)
            {
                await writer.Write(dispatcherContext.Metadata, dispatcherContext.MetadataPublishContext);
            }
        }
    }
}