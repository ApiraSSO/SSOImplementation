namespace Kernel.Cache
{
    using System;

    public interface ICachePopulator<T> : ICachePopulator
    {
        bool IsStale();

        T Refresh(ICacheItemPolicy policy);
    }
}