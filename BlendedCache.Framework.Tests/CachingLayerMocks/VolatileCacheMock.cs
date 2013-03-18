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

		public virtual void Set<T>(string cacheKey, int cacheDurationSeconds, T data) where T : class
		{
			_cache[cacheKey] = data;
		}

		public virtual void Set<T>(string cacheKey, T data) where T : class
		{
			_cache[cacheKey] = data;
		}

		public virtual T Get<T>(string cacheKey) where T : class
		{
			if (_cache.ContainsKey(cacheKey))
				return _cache[cacheKey] as T;

			return null;
		}

		public void Remove(string cacheKey)
		{
			if (_cache.ContainsKey(cacheKey))
				_cache.Remove(cacheKey);
		}
	}
}
