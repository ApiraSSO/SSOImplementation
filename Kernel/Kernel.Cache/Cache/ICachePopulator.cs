namespace Kernel.Cache
{
    using System;

    /// <summary>
    /// Interface ICachePopulator
    /// </summary>
    public interface ICachePopulator : IDisposable
	{
		/// <summary>
		/// Gets the cache key.
		/// </summary>
		/// <value>The cache key.</value>
		string CacheKey { get; }

		/// <summary>
		/// Populates this instance.
		/// </summary>
		void Populate();
	}
}