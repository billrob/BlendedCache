using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Implementation the BlendedCache object uses to interact with looking up items in IVolatileCache.
	/// </summary>
	internal class DefaultVolatileCacheLookup : IVolatileCacheLookup
	{
		private readonly IVolatileCache _volatileCache;
		private readonly IWebRequestCacheMetricsUpdater _metricsUpdater;

		public DefaultVolatileCacheLookup(IVolatileCache volatileCache, IWebRequestCacheMetricsUpdater metricsUpdater)
		{
			_volatileCache = volatileCache;
			_metricsUpdater = metricsUpdater;
		}

		/// <summary>
		/// Will try to get the specified cacheKey from cache as the provided TData type.
		/// </summary>
		/// <typeparam name="TData"></typeparam>
		/// <param name="fixedUpCacheKey">The cacheKey after it has been run through fix up.</param>
		/// <returns></returns>
		public TData GetDataFromVolatileCache<TData>(string fixedUpCacheKey, CacheItemMetrics cacheMetrics) where TData : class
		{
			//get it from volatile.
			var cacheEntry = _volatileCache.Get<TData>(fixedUpCacheKey);

			cacheMetrics.OnItemVolatileCacheLookedUp(cacheEntry, _metricsUpdater);

			return ExtractValidCachedItem<TData>(cacheEntry);
		}

		/// <summary>
		/// Will get a list of cacheKeys from the volatile cache store.  And will return the list of items it found.
		/// </summary>
		public KeyedItemLookupList<TKey,TData> SetDataFromVolatileCache<TData, TKey>(KeyedItemLookupHashSet<TKey, TData> itemsToLookup, SortedList<TKey, TData> foundItems) where TData : class
		{
			var newlyFoundItems = new KeyedItemLookupList<TKey, TData>();

			foreach (var itemToLookup in itemsToLookup.GetRemainingList())
			{
				var cacheEntry = _volatileCache.Get<TData>(itemToLookup.CacheKey);
				itemToLookup.Metrics.OnItemVolatileCacheLookedUp(cacheEntry, _metricsUpdater);

				var cachedItem = ExtractValidCachedItem<TData>(cacheEntry);

				//expired or something
				if (cachedItem == null)
					continue;
			
				foundItems.Add(itemToLookup.LookupKey, cachedItem);
				itemToLookup.CachedItem = cachedItem;
				newlyFoundItems.Add(itemToLookup);
			}

			return newlyFoundItems;
		}

		private static TData ExtractValidCachedItem<TData>(IVolatileCacheEntry<TData> cacheEntry) where TData : class
		{
			//bail out if null
			if (cacheEntry == null)
				return null;

			//this is where blended cache enforces its own definition
			var now = DateTime.UtcNow;
			if (now >= cacheEntry.ExpirationDateTimeUtc)
				return null;

			return cacheEntry.CachedItem;
		}
	}
}
