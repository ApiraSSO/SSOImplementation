using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Kernel.CQRS.MessageHandling;
using Kernel.DependancyResolver;
using Kernel.Extensions;
using Kernel.Reflection;

namespace CQRS.MessageHandling.Factories
{
    internal class HandlerResolver : IHandlerResolver
    {
        private readonly IDependencyResolver _resolver;
        private readonly IHandlerResolverSettings _factorySettings;
        
        public HandlerResolver(IDependencyResolver resolver, IHandlerResolverSettings factorySettings)
        {
            _resolver = resolver;
            _factorySettings = factorySettings;
        }

        public ICollection<object> ResolveAllHandlersFor(Type targetType)
        {
            return ResolveHandlersFor(targetType, (t, s) => true);
        }

        public ICollection<object> ResolveHandlersFor(Type targetType, Func<Type, IHandlerResolverSettings, bool> filter)
        {
            var handlerType = BuildHandlerType(targetType);
            return GetHandlersInternal(handlerType, filter);
        }
        
        protected virtual ICollection<object> GetHandlersInternal(Type handlerType, Func<Type, IHandlerResolverSettings, bool> filter)
        {
            var handlers = ResolveHandlers(handlerType, filter);
            return handlers;
        }

        protected virtual Type BuildHandlerType(Type type)
        {
            var handlerType = typeof(IMessageHandler<>)
               .MakeGenericType(type);
            return handlerType;
        }
        
        protected virtual ICollection<object> ResolveHandlers(Type handlerType, Func<Type, IHandlerResolverSettings, bool> filter)
        {
            //object handler;
            var handlers = this._resolver.ResolveAll(handlerType)
                .Where(h => filter(h.GetType(), this._factorySettings))
                .ToList();

            if (handlers.Count > 0)
                return handlers;

            handlers = TryResolveFromAssemblies(handlerType, filter)
                .ToList();

            if (handlers.Count == 0)
                throw new InvalidOperationException($"No command handler of type: {handlerType.FullName} found.");

            return handlers;
        }

        private ICollection<object> TryResolveFromAssemblies(Type handlerType, Func<Type, IHandlerResolverSettings, bool> filter)
        {
            var scannableAsseblies = this._factorySettings.HasCustomAssemlyList ? 
                this._factorySettings.LimitAssembliesTo :
                AssemblyScanner.ScannableAssemblies;
           
            var implementors = ReflectionHelper.GetAllTypes(scannableAsseblies, t => !t.IsAbstract && !t.IsInterface && TypeExtensions.IsAssignableToGenericType(t, handlerType) && filter(t, this._factorySettings));

            var root = new List<object>();
            var instances = implementors.Aggregate(root, (c, next) => { c.Add(this.CreateInstance(next)); return c; });
            return instances;
        }

        private object CreateInstance(Type type)
        {
            var ctors = type.GetConstructors();
            var ctor = ctors.OrderByDescending(c => c.GetParameters().Length).First();
            var parameters = ctor.GetParameters();
            var pars = new List<ParameterExpression>();
            var resolvedParams = new List<object>();
            foreach (var p in parameters)
            {
                object parInstance;
                var canResolve = _resolver.TryResolve(p.ParameterType, out parInstance);
                if (!canResolve)
                    throw new InvalidOperationException($"Cannot resolve dependency for type: {type.Name}. Dependency type: {p.ParameterType.Name}");
                resolvedParams.Add(parInstance);
                pars.Add(Expression.Parameter(p.ParameterType));
            }
            var ctorExpression = Expression.New(ctor, pars);
            var lambda = Expression.Lambda(ctorExpression, pars);
            var instance = lambda.Compile().DynamicInvoke(resolvedParams.ToArray());
            return instance;
        }
    }
}