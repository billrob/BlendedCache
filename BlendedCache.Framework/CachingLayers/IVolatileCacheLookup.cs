using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Internal class to make testing easier and to break up the actual lookup logic for context cache.
	/// </summary>
	internal interface IVolatileCacheLookup
	{
		/// <summary>
		/// Will try to get the specified cacheKey from cache as the provided TData type.
		/// </summary>
		/// <typeparam name="TData">The type of the object. Normally infered from datatype.</typeparam>
		/// <param name="fixedUpCacheKey">The cacheKey after it has been run through fix up.</param>
		/// <param name="cacheMetrics">The cache metrics item for the given key.</param>
		/// <returns>The existing item if it exists.</returns>
		TData GetDataFromVolatileCache<TData>(string fixedUpCacheKey, CacheItemMetrics cacheMetrics) where TData : class;

		/// <summary>
		/// Will get a list of cacheKeys from the volatile cache store.  And will return the list of items it found.
		/// </summary>
		KeyedItemLookupList<TKey,TData> SetDataFromVolatileCache<TData, TKey>(KeyedItemLookupHashSet<TKey, TData> itemsToLookup, SortedList<TKey, TData> foundItems) where TData : class;
	}
}
