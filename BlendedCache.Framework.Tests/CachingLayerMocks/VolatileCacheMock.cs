using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Tests
{
	internal class VolatileCacheMock : IVolatileCache
	{
		private Dictionary<string, object> _cache = new Dictionary<string, object>();

		public virtual void Set<TData>(string cacheKey, IVolatileCacheEntry<TData> cacheEntry) where TData : class
		{
			_cache[cacheKey] = cacheEntry.CachedItem;
		}

		public virtual TData Get<TData>(string cacheKey) where TData : class
		{
			if (_cache.ContainsKey(cacheKey))
				return _cache[cacheKey] as TData;

			return null;
		}

		public void Remove(string cacheKey)
		{
			if (_cache.ContainsKey(cacheKey))
				_cache.Remove(cacheKey);
		}
	}
}
