using System.Threading.Tasks;
using Kernel.Federation.MetaData.Configuration;

namespace Kernel.Federation.MetaData
{
    public interface IMetadataGenerator
    {
        Task CreateMetadata(MetadataContext metadataContext);
        Task CreateMetadata(MetadataType metadataType);
    }
}