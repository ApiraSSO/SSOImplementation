using System;

namespace Kernel.Cache
{
	public interface ICacheItemPolicy
	{
        DateTimeOffset AbsoluteExpiration { get; set; }
        TimeSpan SlidingExpiration { get; set; }
    }
}