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
	internal interface ICacheMetricsContainer
	{
		/// <summary>
		/// Will get all the cache metrics stored in the container in no particular order.  Not very performant.
		/// </summary>
		/// <returns></returns>
		List<Metrics> GetCacheMetrics();

		/// <summary>
		/// Will get a specific cachekey metric.  Very performant compared to get all cache metrics.
		/// </summary>
		/// <param name="cacheKey">The cacheKey to get the metrics for.</param>
		/// <returns></returns>
		Metrics GetCacheMetrics(string cacheKey);
	}
}
