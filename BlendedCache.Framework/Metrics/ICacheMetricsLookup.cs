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
	/// <typeparam name="TDataLoader">The type of data loader such as DataContext.</typeparam>
	internal interface ICacheMetricsLookup<TDataLoader>
	{
		/// <summary>
		/// Will create a cached item record in the static lock box for recording of metrics of the item.
		/// It could also contain the loader delegate.  If we need to not do a lazy refresh, the loader 
		/// will need to be stored here.
		/// </summary>
		/// <param name="cacheKey">The cacheKey of the item being retrieved.</param>
		/// <returns>Will never return null.</returns>
		CacheItemMetrics GetOrCreateCacheItemMetric(string cacheKey);
	}
}