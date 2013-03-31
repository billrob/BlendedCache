using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Providers.VolatileCache
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
		/// <param name="cachedItem">The actual item to be stored in volatile cache.</param>
		/// <param name="cacheDurationSeconds">The duration in seconds this item should be stored.</param>
		void IVolatileCache.Set<TData>(string key, TData cachedItem, int cacheDurationSeconds)
		{
			var expirationUtc = DateTime.UtcNow.AddSeconds(cacheDurationSeconds);
			MemoryCache.Default.Set(key, cachedItem, new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(expirationUtc) });
		}

		/// <summary>
		/// Will get a strongly typed object from volatile cache.  Will return null if the item does not exist.
		/// </summary>
		/// <typeparam name="TData">The type of object to retreive.</typeparam>
		/// <param name="cacheKey">The cache key of the cached item.</param>
		/// <returns>Will return the item from cache or null if it doesn't exist.</returns>
		TData IVolatileCache.Get<TData>(string cacheKey)
		{
			return MemoryCache.Default.Get(cacheKey) as TData;
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
