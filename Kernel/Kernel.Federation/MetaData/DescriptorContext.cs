using System;

namespace Kernel.Federation.MetaData
{
    public class DescriptorContext
    {
        public DescriptorContext(Type descriptorType)
        {
            this.DescriptorType = descriptorType;
        }

        public Type DescriptorType { get; }
    }
}