using System.Runtime.Caching;
using Kernel.Cache;
using Kernel.Cache.Cache;

namespace MemoryCacheProvider.Dependencies
{
    public abstract class DependencyController : ICacheDependencyController
    {
        public ICacheItemPolicy RegisterDependency(bool registerMonitor)
        {
            var policy = new MemoryCacheItemPolicy();

            if (registerMonitor)
            {
                var monitor = GetChangeMonitor();

                policy.ChangeMonitors.Add(monitor);
            }

            return policy;
        }

        protected abstract ChangeMonitor GetChangeMonitor();
    }
}