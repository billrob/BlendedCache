using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Framework.IntegrationTests
{
	/// <summary>
	/// test class for the dictionary context cache, not to be used in production, ever.
	/// </summary>
	internal class DictionaryContextCache : IContextCache
	{
		private Dictionary<string, object> _collection = new Dictionary<string, object>();

		/// <summary>
		/// Will create an empty dictionary context cache.
		/// </summary>
		public DictionaryContextCache() { }
		
		/// <summary>
		/// Will create a dictionary context cache with the item populated.
		/// </summary>
		/// <param name="cacheKey">The cacheKey to store.</param>
		/// <param name="cachedItem">The cachedItem to store.</param>
		public DictionaryContextCache(string cacheKey, object cachedItem)
		{
			_collection.Add(cacheKey, cachedItem);
		}

		void IContextCache.Set<T>(string key, T value)
		{
			lock (_collection)
				_collection[key] = value;
		}

		T IContextCache.Get<T>(string cacheKey)
		{
			lock (_collection)
			{
				if (!_collection.ContainsKey(cacheKey))
					return null;
				return _collection[cacheKey] as T;
			}
		}

		void IContextCache.Remove(string cacheKey)
		{
			lock (_collection)
			{
				if (_collection.ContainsKey(cacheKey))
					_collection.Remove(cacheKey);
			}
		}
		void IContextCache.Clear()
		{
			_collection.Clear();
		}
	}
}
