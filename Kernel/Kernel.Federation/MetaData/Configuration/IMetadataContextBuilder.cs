using System;

namespace Kernel.Federation.MetaData.Configuration
{
    public interface IMetadataContextBuilder : IDisposable
    {
        MetadataContext BuildContext(MetadataGenerateRequest metadataGenerateContext);
    }
}