using System;
using System.Data.Entity.Core.Objects;
using Kernel.Resolvers;

namespace Provider.EntityFramework.Resolvers
{
    internal class TypeProviderService : ITypeResolver
    {
        public Type ResolverUnderlyingType(Type type)
        {
            return ObjectContext.GetObjectType(type);
        }
    }
}