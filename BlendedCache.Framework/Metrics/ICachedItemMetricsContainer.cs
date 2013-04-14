using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Interface marking an object that contains cache metrics.
	/// </summary>
	internal interface ICachedItemMetricsContainer // change to lookupKey if this goes public.
	{
		/// <summary>
		/// Will get all the cache metrics stored in the container in no particular order.  Not very performant.
		/// </summary>
		/// <returns></returns>
		List<Metrics> GetCacheMetrics();

		/// <summary>
		/// Will get a specific cachedItem metric by cacheKey.  Very performant compared to get all cache metrics.  Can return null.
		/// </summary>
		/// <param name="cacheKey">The cacheKey to get the metrics for.</param>
		/// <returns></returns>
		Metrics GetCachedItemMetrics(string cacheKey); //this doesn't use lookupKey because I was worried about the equality/GetHashCode implementations.
	}
}
