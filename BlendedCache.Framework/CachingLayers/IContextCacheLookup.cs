using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheKey = System.String;

namespace BlendedCache
{
	/// <summary>
	/// Internal class to make testing easier and to break up the actual lookup logic for context cache.
	/// </summary>
	internal interface IContextCacheLookup
	{
		/// <summary>
		/// Will try to get the specified cacheKey from cache as the provided TData type.
		/// </summary>
		/// <typeparam name="TData"></typeparam>
		/// <param name="cacheKey">The cacheKey after it has been run through fix up.</param>
		/// <returns>The existing item if it exists.</returns>
		TData GetDataFromContextCache<TData>(string cacheKey) where TData : class;

		/// <summary>
		/// Will get a dictionary of the items that can be found in context cache.
		/// </summary>
		/// <typeparam name="TData">The type of item to load.</typeparam>
		/// <param name="cacheKeys">The list of cacheKeys to load up.</param>
		/// <returns></returns>
		void SetDataFromContextCache<TData, TKey>(KeyedItemLookupHashSet<TKey, TData> itemsToLookup, SortedList<TKey, TData> foundItems) where TData : class;
	}
}
