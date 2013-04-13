using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Implementation the BlendedCache object uses to interact with looking up items in ILongTermCache.
	/// </summary>
	internal class DefaultLongTermCacheLookup : ILongTermCacheLookup
	{
		private readonly ILongTermCache _longTermCache;
		private readonly IWebRequestCacheMetricsUpdater _metricsUpdater;

		public DefaultLongTermCacheLookup(ILongTermCache longTermCache, IWebRequestCacheMetricsUpdater metricsUpdater)
		{
			_longTermCache = longTermCache;
			_metricsUpdater = metricsUpdater;
		}

		/// <summary>
		/// Will try to get the specified cacheKey from cache as the provided TData type.
		/// </summary>
		/// <typeparam name="TData"></typeparam>
		/// <param name="fixedUpCacheKey">The cacheKey after it has been run through fix up.</param>
		/// <returns></returns>
		public TData GetDataFromLongTermCache<TData>(string fixedUpCacheKey, CacheItemMetrics cacheMetrics) where TData : class
		{
			//get it from http LongTerm.
			var cacheEntry = _longTermCache.Get<TData>(fixedUpCacheKey);

			cacheMetrics.OnItemLongTermCacheLookedUp(cacheEntry, _metricsUpdater);

			return ExtractValidCachedItem<TData>(cacheEntry);
		}

		/// <summary>
		/// Will set the long term items found in the collection and return the list that was found.
		/// </summary>
		public KeyedItemLookupList<TKey, TData> SetDataFromLongTermCache<TData, TKey>(KeyedItemLookupHashSet<TKey, TData> itemsToLookup, SortedList<TKey, TData> foundItems) where TData : class
		{
			var newlyFoundItems = new KeyedItemLookupList<TKey, TData>();

			var remainingItems = itemsToLookup.GetRemainingList();

			var foundCachedItems = _longTermCache.Get<TData>(remainingItems.Select(x => x.CacheKey));

			foreach (var itemToLookup in remainingItems)
			{
				var cacheEntry = (ILongTermCacheEntry<TData>)null;

				//not found.
				foundCachedItems.TryGetValue(itemToLookup.CacheKey, out cacheEntry);
				itemToLookup.Metrics.OnItemLongTermCacheLookedUp(cacheEntry, _metricsUpdater);

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

		private static TData ExtractValidCachedItem<TData>(ILongTermCacheEntry<TData> cacheEntry) where TData : class
		{
			//no entry found
			if (cacheEntry == null)
				return null;

			var now = DateTime.UtcNow;
			if (now >= cacheEntry.ExpirationDateTimeUtc)
				//todo: unless database is down.
				return null;

			if (now >= cacheEntry.RefreshDateTimeUtc)
				;//todo: queue off refresh expiration execution

			return cacheEntry.CachedItem;
		}
	}
}
