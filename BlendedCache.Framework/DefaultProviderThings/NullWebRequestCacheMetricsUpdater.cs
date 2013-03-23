using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Null mock class for the web request cache metrics updater.
	/// </summary>
	internal class NullWebRequestCacheMetricsUpdater : IWebRequestCacheMetricsUpdater
	{
		void IWebRequestCacheMetricsUpdater.Increment_Cache_LongTermHits()
		{
		}

		void IWebRequestCacheMetricsUpdater.Increment_Cache_LongTermLookup()
		{
		}

		void IWebRequestCacheMetricsUpdater.Increment_Cache_LongTermMisses()
		{
		}

		void IWebRequestCacheMetricsUpdater.Increment_Cache_VolatileHits()
		{
		}

		void IWebRequestCacheMetricsUpdater.Increment_Cache_VolatileLookup()
		{
		}

		void IWebRequestCacheMetricsUpdater.Increment_Cache_VolatileMisses()
		{
		}
	}
}
