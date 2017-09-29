using System;

namespace Kernel.Federation.MetaData
{
    [Flags]
    public enum MetadataPublishProtocol
    {
        Memory= 0,
        Http = 1,
        FileSystem = 2
    }
}