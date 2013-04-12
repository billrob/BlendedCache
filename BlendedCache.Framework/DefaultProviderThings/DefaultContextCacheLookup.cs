using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheKey = System.String;

namespace BlendedCache
{
	/// <summary>
	/// Implementation the BlendedCache object uses to interact with looking up items in IContextCache.
	/// </summary>
	internal class DefaultContextCacheLookup : IContextCacheLookup
	{
		private readonly IContextCache _contextCache;

		public DefaultContextCacheLookup(IContextCache contextCache)
		{
			_contextCache = contextCache;
		}

		/// <summary>
		/// Will try to get the specified cacheKey from cache as the provided TData type.
		/// </summary>
		/// <typeparam name="TData"></typeparam>
		/// <param name="cacheKey">The cacheKey after it has been run through fix up.</param>
		/// <returns></returns>
		public TData GetDataFromContextCache<TData>(string cacheKey) where TData : class
		{
			//return it from http context.
			return _contextCache.Get<TData>(cacheKey);
		}


		/// <summary>
		/// Will get a dictionary of the items that can be found in context cache.
		/// </summary>
		/// <typeparam name="TData">The type of item to load.</typeparam>
		/// <param name="itemsToLookup">The list of cache/lookup keys to load up.</param>
		/// <returns></returns>
		public void SetDataFromContextCache<TData, TKey>(KeyedItemLookupHashSet<TKey, TData> itemsToLookup, SortedList<TKey, TData> foundItems) where TData : class
		{
			//grab what we can from the http context
			foreach (var itemToLookup in itemsToLookup)
			{
				TData cachedItem = GetDataFromContextCache<TData>(itemToLookup.CacheKey);
				if (cachedItem != null)
				{
					itemToLookup.CachedItem = cachedItem;
					foundItems.Add(itemToLookup.LookupKey, cachedItem);
				}
			}
		}
	}
}
