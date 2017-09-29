using System.Threading.Tasks;
using System.Xml;

namespace Kernel.Federation.MetaData
{
    public interface IFederationMetadataWriter
    {
        Task Write(XmlElement xml, MetadataPublishContext target);
    }
}