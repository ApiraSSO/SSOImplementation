using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Linq.Expressions;
using Kernel.Federation.MetaData;
using Kernel.Federation.MetaData.Configuration.RoleDescriptors;
using Kernel.Reflection;

namespace WsFederationMetadataProvider.Metadata.DescriptorBuilders
{
    internal class DescriptorBuildersHelper
    {
        private static ConcurrentDictionary<Type, Func<object, RoleDescriptorConfiguration, RoleDescriptor>> _delCache = new ConcurrentDictionary<Type, Func<object, RoleDescriptorConfiguration, RoleDescriptor>>();
        public static object ResolveDescriptor(Type type)
        {
            var genericType = typeof(IDescriptorBuilder<>).MakeGenericType(type);
            var builder = DescriptorBuildersHelper.GetTypes(t => !t.IsAbstract && !t.IsInterface && genericType.IsAssignableFrom(t))
                .FirstOrDefault();
            if (builder == null)
                throw new InvalidOperationException(String.Format("Descriptor builder for type: {0} not found.", genericType.Name));
            return Activator.CreateInstance(builder);
        }

        public static RoleDescriptor ResolveAndBuild(RoleDescriptorConfiguration configuration)
        {
            var descriptor = DescriptorBuildersHelper.ResolveDescriptor(configuration.RoleDescriptorType);
            var del = DescriptorBuildersHelper.GetDescriptorDelegate(configuration.RoleDescriptorType);
            return del(descriptor, configuration);
        }

        public static Func<object, RoleDescriptorConfiguration, RoleDescriptor> GetDescriptorDelegate(Type type)
        {
            return DescriptorBuildersHelper._delCache.GetOrAdd(type, t => DescriptorBuildersHelper.BuildDelegate(t));
        }

        private static Func<object, RoleDescriptorConfiguration, RoleDescriptor> BuildDelegate(Type type)
        {
            var genericType = typeof(IDescriptorBuilder<>).MakeGenericType(type);
            var methodInfo = genericType.GetMethod("BuildDescriptor");
            var descParam = Expression.Parameter(typeof(object));
            var confgPar = Expression.Parameter(typeof(RoleDescriptorConfiguration));
            var castExpression = Expression.Convert(descParam, genericType);
            var callExp = Expression.Call(castExpression, methodInfo, confgPar);
            var lambda = Expression.Lambda<Func<object, RoleDescriptorConfiguration, RoleDescriptor>>(callExp, descParam, confgPar);
            return lambda.Compile();
        }

        private static IEnumerable<Type> GetTypes(Func<Type, bool> func)
        {
            return ReflectionHelper.GetAllTypes(func);
        }
    }
}