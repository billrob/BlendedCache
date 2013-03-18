using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Tests
{
	internal class ContextCacheMock : IContextCache
	{
		private Dictionary<string, object> _cache = new Dictionary<string, object>();

		public virtual void Set<T>(string key, T value) where T : class
		{
			_cache[key] = value;
		}

		public virtual T Get<T>(string key) where T : class
		{
			if (_cache.ContainsKey(key))
				return _cache[key] as T;

			return null;
		}

		public virtual IEnumerable<string> Keys
		{
			get { return _cache.Keys.ToList().AsEnumerable(); }
		}

		public virtual void Remove(string key)
		{
			if (_cache.ContainsKey(key))
				_cache.Remove(key);
		}
	}
}