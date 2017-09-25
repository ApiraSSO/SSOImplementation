using System.Collections.Generic;
using System.Runtime.Caching;

namespace MemoryCacheProvider.Dependencies
{
    public abstract class FileDependencyController : DependencyController
    {
        protected abstract IList<string> FilePaths { get; }

        protected override ChangeMonitor GetChangeMonitor()
        {
            var monitor = new HostFileChangeMonitor(FilePaths);

            return monitor;
        }
    }
}