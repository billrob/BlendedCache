using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Framework.IntegrationTests
{
	/// <summary>
	/// test class for the dictionary longterm cache, not to be used in production, ever.
	/// </summary>
	internal class DictionaryLongTermCache : ILongTermCache
	{
		private Dictionary<string, DefaultLongTermCacheEntry<object>> _collection = new Dictionary<string, DefaultLongTermCacheEntry<object>>();

		/// <summary>
		/// Will create an empty dictionary long term cache.
		/// </summary>
		public DictionaryLongTermCache() { }

		/// <summary>
		/// Will create a dictionary long term cache with the item populated.
		/// </summary>
		/// <param name="cacheKey">The cacheKey to store.</param>
		/// <param name="cachedItem">The cachedItem to store.</param>
		public DictionaryLongTermCache(string cacheKey, object cachedItem)
		{
			var item = new DefaultLongTermCacheEntry<object>(cachedItem, 60, 60);

			_collection.Add(cacheKey, item);
		}


		ILongTermCacheEntry<TData> ILongTermCache.Get<TData>(string cacheKey)
		{
			lock (_collection)
			{
				if (!_collection.ContainsKey(cacheKey))
					return null;

				var cacheEntry = _collection[cacheKey];

				//the higher layers do this already
				if (DateTime.UtcNow > cacheEntry.ExpirationDateTimeUtc)
				{
					_collection.Remove(cacheKey);
					return null;
				}

				var cachedItem = cacheEntry.CachedItem as TData;
				if (cachedItem == null)
					return null;

				return new DefaultLongTermCacheEntry<TData>(cachedItem, cacheEntry.ExpirationDateTimeUtc, cacheEntry.RefreshDateTimeUtc);
			}
		}

		void ILongTermCache.Set<TData>(string cacheKey, ILongTermCacheEntry<TData> cacheEntry)
		{
			var item = new DefaultLongTermCacheEntry<object>(cacheEntry.CachedItem, cacheEntry.ExpirationDateTimeUtc, cacheEntry.RefreshDateTimeUtc);

			lock (_collection)
				_collection[cacheKey] = item;
		}


		IDictionary<string, ILongTermCacheEntry<TData>> ILongTermCache.Get<TData>(IEnumerable<string> cacheKeys)
		{
			var dictionary = new Dictionary<string, ILongTermCacheEntry<TData>>();

			foreach (var cacheKey in cacheKeys)
			{
				var cachedEntry = ((ILongTermCache)this).Get<TData>(cacheKey);
				if (cachedEntry != null)
					dictionary.Add(cacheKey, cachedEntry);
			}

			return dictionary;
		}
	}
}