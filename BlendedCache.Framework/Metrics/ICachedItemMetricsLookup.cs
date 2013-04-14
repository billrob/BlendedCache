using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// The cache metrics lookup object.
	/// </summary>
	internal interface ICachedItemMetricsLookup
	{
		/// <summary>
		/// Will create a cached item record in the static lock box for recording of metrics of the item.
		/// It could also contain the loader delegate.  If we need to not do a lazy refresh, the loader 
		/// will need to be stored here.
		/// </summary>
		/// <param name="cacheKey">The cache key for the item.</param>
		/// <param name="lookupKey">The lookupKey of the item being retrieved.</param>
		/// <returns>Will never return null.</returns>
		CachedItemMetrics GetOrCreateCacheItemMetric<TData, TKey>(string cacheKey, TKey lookupKey); //only complex because an internal method.
	}
}