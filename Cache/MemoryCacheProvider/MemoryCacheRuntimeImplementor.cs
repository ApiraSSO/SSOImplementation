using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Kernel.Cache;

namespace MemoryCacheProvider
{
    public class MemoryCacheRuntimeImplementor : ICacheProvider
    {
        /// <summary>
        /// Occurs when [written to].
        /// </summary>
        public event EventHandler WrittenTo;

        /// <summary>
        /// Occurs when [read from].
        /// </summary>
        public event EventHandler ReadFrom;

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public object Get(string key)
        {
            key = this.FormatKey(key);

            try
            {
                return MemoryCache.Default.Get(key);
            }
            catch
            {
                throw new Exception(string.Format("Key '{0}' not found.", key));
            }
            finally
            {
                OnReadFrom();
            }
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// </exception>
        public T Get<T>(string key)
        {
            key = this.FormatKey(key);

            try
            {
                return (T)MemoryCache.Default.Get(key);
            }
            catch (InvalidCastException)
            {
                throw new Exception(string.Format("Key '{0}' value type mismatch.", key));
            }
            catch
            {
                throw new Exception(string.Format("Key '{0}' not found.", key));
            }
            finally
            {
                OnReadFrom();
            }
        }

        /// <summary>
        /// Inserts value with specified key or updates if it already exists
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Put(string key, object value)
        {
            var policy = new MemoryCacheItemPolicy();

            this.Put(key, value, policy);
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="policy"></param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public void Put(string key, object value, ICacheItemPolicy policy)
        {
            var cachePolicy = policy as CacheItemPolicy;
            if (cachePolicy == null)
                throw new InvalidOperationException(String.Format("Expected type: {0}, but was: {1}", typeof(CacheItemPolicy).Name, policy.GetType().Name));

            key = this.FormatKey(key);

            MemoryCache.Default.Set(key, value, cachePolicy);

            OnWrittenTo();
        }

        /// <summary>
        /// Deletes the value specified at key location.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object Delete(string key)
        {
            key = this.FormatKey(key);

            var value = MemoryCache.Default.Remove(key);

            return value;
        }

        /// <summary>
        /// Creates an entry at the given key, throws an exception if that entry already exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Post(string key, object value)
        {
            var policy = new MemoryCacheItemPolicy();

            Post(key, value, policy);
        }

        /// <summary>
        /// Creates an entry at the given key, throws an exception if that entry already exists.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="policy"></param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public void Post(string key, object value, ICacheItemPolicy policy)
        {
            var cachePolicy = policy as CacheItemPolicy;
            if (cachePolicy == null)
                throw new InvalidOperationException(String.Format("Expected type: {0}, but was: {1}", typeof(CacheItemPolicy).Name, policy.GetType().Name));

            key = FormatKey(key);

            MemoryCache.Default.Add(key, value, cachePolicy);

            OnWrittenTo();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initialises this instance.
        /// </summary>
        public void Initialise()
        {
        }

        /// <summary>
        /// Determines whether the cache contains anything with the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string key)
        {
            key = this.FormatKey(key);

            return MemoryCache.Default.Contains(key);
        }

        /// <summary>
        /// Types the of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IDictionary<string, T> TypeOf<T>()
        {
            return MemoryCache.Default.Where(i => i.Value.GetType() == typeof(T))
                .ToDictionary(k => k.Key, v => (T)v.Value);
        }

        /// Formats the key - keys are case sensitive!
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private string FormatKey(string key)
        {
            return key.ToLower();
        }

        /// <summary>
        /// Called when [written to].
        /// </summary>
        protected virtual void OnWrittenTo()
        {
            if (WrittenTo != null)
            {
                WrittenTo(this, new EventArgs());
            }
        }

        /// <summary>
        /// Called when [read from].
        /// </summary>
        protected virtual void OnReadFrom()
        {
            if (ReadFrom != null)
            {
                ReadFrom(this, new EventArgs());
            }
        }
    }
}
