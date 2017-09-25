using Kernel.Federation.MetaData.Configuration.RoleDescriptors;

namespace Kernel.Federation.MetaData
{
    public interface IDescriptorBuilder<T> where T : class
    {
        T BuildDescriptor(RoleDescriptorConfiguration configuration);
    }
}