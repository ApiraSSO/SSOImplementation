using System;
using System.Collections.Generic;

namespace Kernel.CQRS.MessageHandling
{
    public interface IHandlerResolver
    {
        ICollection<object> ResolveAllHandlersFor(Type targetType);
        
        ICollection<object> ResolveHandlersFor(Type targetType, Func<Type, IHandlerResolverSettings, bool> filter);
    }
}
