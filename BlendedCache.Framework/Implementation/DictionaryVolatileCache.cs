using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Provides a dictionary based volatile cache.  This is a in-memory cache scoped to the instance of this variable.
	/// </summary>
	public class DictionaryVolatileCache : IVolatileCache
	{
		private IDictionary<string, DefaultVolatileCacheEntry<object>> _collection = new ConcurrentDictionary<string, DefaultVolatileCacheEntry<object>>();

		/// <summary>
		/// Will create an empty dictionary volatile cache.
		/// </summary>
		public DictionaryVolatileCache() { }

		/// <summary>
		/// Will create a dictionary volatile cache with the item populated.
		/// </summary>
		/// <param name="cacheKey">The cacheKey to store.</param>
		/// <param name="cachedItem">The cachedItem to store.</param>
		internal DictionaryVolatileCache(string cacheKey, object cachedItem)
		{
			var item = new DefaultVolatileCacheEntry<object>(cachedItem, 60);

			_collection.Add(cacheKey, item);
		}

		public void Set<TData>(string cacheKey, IVolatileCacheEntry<TData> cacheEntry) where TData : class
		{
			var item = new DefaultVolatileCacheEntry<object>(cacheEntry.CachedItem, cacheEntry.ExpirationDateTimeUtc);
			lock (_collection)
				_collection[cacheKey] = item;
		}

		public IVolatileCacheEntry<TData> Get<TData>(string cacheKey) where TData : class
		{
			lock (_collection)
			{
				if (!_collection.ContainsKey(cacheKey))
					return null;

				var wrappedCacheItem = _collection[cacheKey];

				//the lookup should also do this.
				if (DateTime.UtcNow >= wrappedCacheItem.ExpirationDateTimeUtc)
				{
					_collection.Remove(cacheKey);
					return null;
				}

				var cachedItem = wrappedCacheItem.CachedItem as TData;
				if (cachedItem == null)
					return null;

				return new DefaultVolatileCacheEntry<TData>(wrappedCacheItem.CachedItem as TData, wrappedCacheItem.ExpirationDateTimeUtc);
			}
		}

		public void Remove(string cacheKey)
		{
			lock (_collection)
			{
				if (_collection.ContainsKey(cacheKey))
					_collection.Remove(cacheKey);
			}
		}
	}
}
