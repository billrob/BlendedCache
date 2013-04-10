using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Implementation the BlendedCache object uses to interact with looking up items in IVolatileCache.
	/// </summary>
	internal class DefaultVolatileCacheLookup : IVolatileCacheLookup
	{
		private readonly IVolatileCache _volatileCache;
		private readonly IWebRequestCacheMetricsUpdater _metricsUpdater;

		public DefaultVolatileCacheLookup(IVolatileCache volatileCache, IWebRequestCacheMetricsUpdater metricsUpdater)
		{
			_volatileCache = volatileCache;
			_metricsUpdater = metricsUpdater;
		}

		/// <summary>
		/// Will try to get the specified cacheKey from cache as the provided TData type.
		/// </summary>
		/// <typeparam name="TData"></typeparam>
		/// <param name="fixedUpCacheKey">The cacheKey after it has been run through fix up.</param>
		/// <returns></returns>
		public TData GetDataFromVolatileCache<TData>(string fixedUpCacheKey, CacheItemMetrics cacheMetrics) where TData : class
		{
			//get it from volatile.
			var item = _volatileCache.Get<TData>(fixedUpCacheKey);

			cacheMetrics.OnItemVolatileCacheLookedUp(item, _metricsUpdater);

			//bail out if null
			if (item == null)
				return null;

			//this is where blended cache enforces its own definition
			var now = DateTime.UtcNow;
			if (now >= item.ExpirationDateTimeUtc)
				return null;

			return item.CachedItem;
		}
	}
}
