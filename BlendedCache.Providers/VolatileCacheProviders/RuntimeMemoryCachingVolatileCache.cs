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
		//todo:0 update documentation.
		/// <summary>
		/// Will add the data to cache with the expiration time defined in the request.
		/// </summary>
		/// <param name="key">The cache key for the item.</param>
		/// <param name="cacheDurationSeconds">How long the item should be cached for.</param>
		/// <param name="data">The item that should be cached.</param>
		void IVolatileCache.Set<T>(string key, int cacheDurationSeconds, T data)
		{
			var expirationUtc = DateTime.UtcNow.AddSeconds(cacheDurationSeconds);
			MemoryCache.Default.Set(key, data, new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(expirationUtc) });
		}

		public void Set<T>(string key, T data) where T : class
		{
			MemoryCache.Default.Set(key, data, new CacheItemPolicy());
		}

		/// <summary>
		/// Will get the item from cache by the key.  Can return null.
		/// </summary>
		/// <param name="cacheKey">The cache key for the item.</param>
		/// <returns></returns>
		T IVolatileCache.Get<T>(string cacheKey)
		{
			return MemoryCache.Default.Get(cacheKey) as T;
		}

		void IVolatileCache.Remove(string cacheKey)
		{
			MemoryCache.Default.Remove(cacheKey);
		}
	}
}
