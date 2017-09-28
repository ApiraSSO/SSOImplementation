using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Federation.MetaData
{
    public interface IMetadataHandler<TMetadata>
    {
        Uri ReadIdpLocation(TMetadata metadata, Uri binding);
    }
}