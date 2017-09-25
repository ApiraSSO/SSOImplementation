namespace Kernel.Cache
{
	using System;
	using System.Collections.Generic;

	public interface ICacheProvider
	{
		event EventHandler WrittenTo;
		event EventHandler ReadFrom;

		object Get(string key);

		/// <summary>
		/// Gets the specified key.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">The key.</param>
		/// <returns></returns>
		T Get<T>(string key);

		/// <summary>
		/// Inserts value with specified key or updates if it already exists
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		void Put(string key, object value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="policy"></param>

		void Put(string key, object value, ICacheItemPolicy policy);
		/// <summary>
		/// Deletes the value specified at key location.
		/// </summary>
		/// <param name="key">The key.</param>
		object Delete(string key);

		/// <summary>
		/// Creates an entry at the given key, throws an exception if that entry already exists.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		void Post(string key, object value);

		void Post(string key, object value, ICacheItemPolicy policy);

		/// <summary>
		/// Clears the entire cache
		/// </summary>
		void Clear();

		void Initialise();

		/// <summary>
		/// Determines whether the cache contains anything with the given key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>
		///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
		/// </returns>
		bool Contains(string key);

		IDictionary<string, T> TypeOf<T>();
	}
}
