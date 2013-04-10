using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Providers
{
	/// <summary>
	/// Wraps the built in system.runtime (MemoryCache.Default) cache which handles both web and non-web applications.
	/// </summary>
	public class RuntimeMemoryCachingVolatileCache : IVolatileCache
	{
		/// <summary>
		/// Will set the given item in cache with for the cache durations specified.
		/// </summary>
		/// <typeparam name="TData">The type of the object. Normally infered from datatype.</typeparam>
		/// <param name="cacheKey">The cacheKey for the item.</param>
		/// <param name="cacheEntry">The actual item to be stored in volatile cache.</param>
		void IVolatileCache.Set<TData>(string key, IVolatileCacheEntry<TData> cacheEntry)
		{
			MemoryCache.Default.Set(key, cacheEntry, new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(cacheEntry.ExpirationDateTimeUtc) });
		}

		/// <summary>
		/// Will get a strongly typed object from volatile cache.  Will return null if the item does not exist.
		/// </summary>
		/// <typeparam name="TData">The type of object to retreive.</typeparam>
		/// <param name="cacheKey">The cache key of the cached item.</param>
		/// <returns>Will return the item from cache or null if it doesn't exist.</returns>
		IVolatileCacheEntry<TData> IVolatileCache.Get<TData>(string cacheKey)
		{
			return MemoryCache.Default.Get(cacheKey) as IVolatileCacheEntry<TData>;
		}

		/// <summary>
		/// Will remove the specified item from the context cache.  No action is taken if the cacheKey does not exist.
		/// </summary>
		/// <param name="cacheKey">The cacheKey to be removed.</param>
		void IVolatileCache.Remove(string cacheKey)
		{
			MemoryCache.Default.Remove(cacheKey);
		}
	}
}
