using System;

namespace Kernel.Federation.MetaData
{
    public interface IMetadataHandler<TMetadata>
    {
        Uri ReadIdpLocation(TMetadata metadata, Uri binding);
    }
}