using System;
using System.Collections.Generic;

namespace Kernel.Federation.MetaData
{
    public interface IMetadataHandler<TMetadata>
    {
        Uri ReadIdpLocation(TMetadata metadata, Uri binding);
        IEnumerable<TRole> GetRoleDescriptors<TRole>(TMetadata metadata);
    }
}