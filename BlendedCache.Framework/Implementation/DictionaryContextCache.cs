using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BlendedCache
{
	/// <summary>
	/// Provides a dictionary based context based cache.  Very similar to how HttpContext.Items collection works.
	/// It keeps a private field of dictionary so be aware of how the instances of this class are shared.  If using 
	/// IoC, you want to ensure this is created new for each httpcontext or thread based IoC.
	/// </summary>
	public class DictionaryContextCache : IContextCache
	{
		private IDictionary<string, object> _items = new ConcurrentDictionary<string, object>();

		/// <summary>
		/// Will create an empty dictionary context cache.
		/// </summary>
		public DictionaryContextCache()
		{

		}

		/// <summary>
		/// Will create a dictionary context cache with the item populated.
		/// </summary>
		/// <param name="cacheKey">The cacheKey to store.</param>
		/// <param name="cachedItem">The cachedItem to store.</param>
		internal DictionaryContextCache(string cacheKey, object cachedItem)
		{
			_items.Add(cacheKey, cachedItem);
		}

		//todo:0 pull over final docs.
		void IContextCache.Set<T>(string key, T value)
		{
			_items[key] = value;
		}

		T IContextCache.Get<T>(string key)
		{
			object obj;
			return _items.TryGetValue(key, out obj) ? (T)obj : default(T);
		}

		void IContextCache.Clear()
		{
			_items = new ConcurrentDictionary<string, object>();
		}

		void IContextCache.Remove(string key)
		{
			_items.Remove(key);
		}
	}
}
