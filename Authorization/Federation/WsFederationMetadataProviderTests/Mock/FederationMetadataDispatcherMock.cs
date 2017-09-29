using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kernel.Federation.MetaData;

namespace WsFederationMetadataProviderTests.Mock
{
    internal class FederationMetadataDispatcherMock : IFederationMetadataDispatcher
    {
        private readonly Func<IEnumerable<IFederationMetadataWriter>> _writers;
        public FederationMetadataDispatcherMock(Func<IEnumerable<IFederationMetadataWriter>> writers)
        {
            this._writers = writers;
        }
        public async Task Dispatch(DispatcherContext dispatcherContext)
        {
            foreach(var writer in this._writers())
            {
                await writer.Write(dispatcherContext.Metadata, dispatcherContext.MetadataPublishContext);
            }
        }
    }
}