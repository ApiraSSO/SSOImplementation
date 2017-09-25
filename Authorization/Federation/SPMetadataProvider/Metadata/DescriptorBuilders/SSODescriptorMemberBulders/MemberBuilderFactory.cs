using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kernel.Reflection;

namespace WsFederationMetadataProvider.Metadata.DescriptorBuilders.SSODescriptorMemberBulders
{
    internal class MemberBuilderFactory
    {
        private static Func<Type, bool> _condition = t => !t.IsAbstract && !t.IsInterface && typeof(RoleDescriptorMemberBuilder).IsAssignableFrom(t);
        private static Assembly[] _assembliesToSearch = new[] { typeof(RoleDescriptorMemberBuilder).Assembly };
        internal static IEnumerable<RoleDescriptorMemberBuilder> GetBuilders()
        {
            var types = ReflectionHelper.GetAllTypes(MemberBuilderFactory._assembliesToSearch, MemberBuilderFactory._condition);
            return MemberBuilderFactory.Build(types);
        }
        internal static IEnumerable<RoleDescriptorMemberBuilder> Build(IEnumerable<Type> types)
        {
            return types.Select(t => (RoleDescriptorMemberBuilder)Activator.CreateInstance(t));
        }
    }
}