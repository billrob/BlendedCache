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
		/// Will get all the cache metrics stored in the container in no particular order.
		/// </summary>
		/// <returns></returns>
		List<Metrics> GetCacheMetrics();
	}
}
