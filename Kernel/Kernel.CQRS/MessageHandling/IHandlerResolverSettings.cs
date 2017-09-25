using System.Collections.Generic;
using System.Reflection;

namespace Kernel.CQRS.MessageHandling
{
    public interface IHandlerResolverSettings
    {
        IEnumerable<Assembly> LimitAssembliesTo { get; }
        bool HasCustomAssemlyList { get; }
    }
}