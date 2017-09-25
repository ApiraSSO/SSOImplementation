namespace Kernel.Cache.Cache
{
    public interface ICacheDependencyController
    {
        ICacheItemPolicy RegisterDependency(bool registerMonitor);
    }
}