﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Internal class to make testing easier and to break up the actual lookup logic for long term cache.
	/// </summary>
	internal interface ILongTermCacheLookup
	{
		/// <summary>
		/// Will try to get the specified cacheKey from cache as the provided TData type.
		/// </summary>
		/// <typeparam name="TData"></typeparam>
		/// <param name="fixedUpCacheKey">The cacheKey after it has been run through fix up.</param>
		/// <param name="cacheMetrics">The cache metrics item for the given key.</param>
		/// <returns>The existing item if it exists and expiration policies are also verified.</returns>
		TData GetDataFromLongTermCache<TData>(string fixedUpCacheKey, CachedItemMetrics cacheMetrics) where TData : class;

		/// <summary>
		/// Will set the long term items found in the collection and return the list that was found.
		/// </summary>
		KeyedItemLookupList<TKey, TData> SetDataFromLongTermCache<TData, TKey>(KeyedItemLookupHashSet<TKey, TData> itemsToLookup, SortedList<TKey, TData> foundItems) where TData : class;
	}
}
