using Kernel.Federation.MetaData;
using Shared.Federtion;

namespace FileSystemMetadataWriter
{
    internal class MetadataFileWriter : MetadataWriter
    {
        protected override bool CanWrite(MetadataPublishContext target)
        {
            return (target.MetadataPublishProtocol & MetadataPublishProtocol.FileSystem) == MetadataPublishProtocol.FileSystem;
        }
    }
}