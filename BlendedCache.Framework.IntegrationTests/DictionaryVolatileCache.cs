﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Framework.IntegrationTests
{
	/// <summary>
	/// test class for the dictionary volatile cache, not to be used in production, ever.
	/// </summary>
	internal class DictionaryVolatileCache : IVolatileCache
	{
		private Dictionary<string, CacheWrapper> _collection = new Dictionary<string, CacheWrapper>();

		/// <summary>
		/// Will create an empty dictionary volatile cache.
		/// </summary>
		public DictionaryVolatileCache() { }
		
		/// <summary>
		/// Will create a dictionary volatile cache with the item populated.
		/// </summary>
		/// <param name="cacheKey">The cacheKey to store.</param>
		/// <param name="cachedItem">The cachedItem to store.</param>
		public DictionaryVolatileCache(string cacheKey, object cachedItem)
		{
			var item = new CacheWrapper()
			{
				CachedItem = cachedItem,
				ExpirationUtc = DateTime.UtcNow.AddSeconds(60),
			};

			_collection.Add(cacheKey, item);
		}

		void IVolatileCache.Set<TData>(string cacheKey, TData cachedItem, int cacheDurationSeconds)
		{
			var item = new CacheWrapper()
			{
				CachedItem = cachedItem,
				ExpirationUtc = DateTime.UtcNow.AddSeconds(cacheDurationSeconds),
			};

			lock(_collection)
				_collection[cacheKey] = item;
		}

		TData IVolatileCache.Get<TData>(string cacheKey)
		{
			lock (_collection)
			{
				if (!_collection.ContainsKey(cacheKey))
					return null;

				var item = _collection[cacheKey];
				
				if (DateTime.UtcNow > item.ExpirationUtc)
				{
					_collection.Remove(cacheKey);
					return null;
				}

				return item.CachedItem as TData;
			}
		}

		void IVolatileCache.Remove(string cacheKey)
		{
			lock (_collection)
			{
				if (_collection.ContainsKey(cacheKey))
					_collection.Remove(cacheKey);
			}
		}

		private class CacheWrapper
		{
			public object CachedItem { get; set; }
			public DateTime ExpirationUtc { get; set; }
		}
	}
}
