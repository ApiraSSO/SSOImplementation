using System;

namespace Kernel.Resolvers
{
    public interface ITypeResolver
    {
        Type ResolverUnderlyingType(Type type);
    }
}
