using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Implementation the BlendedCache object uses to interact with looking up items in ILongTermCache.
	/// </summary>
	internal class DefaultLongTermCacheLookup : ILongTermCacheLookup
	{
		private readonly ILongTermCache _longTermCache;
		private readonly IWebRequestCacheMetricsUpdater _metricsUpdater;

		public DefaultLongTermCacheLookup(ILongTermCache longTermCache, IWebRequestCacheMetricsUpdater metricsUpdater)
		{
			_longTermCache = longTermCache;
			_metricsUpdater = metricsUpdater;
		}

		/// <summary>
		/// Will try to get the specified cacheKey from cache as the provided TData type.
		/// </summary>
		/// <typeparam name="TData"></typeparam>
		/// <param name="fixedUpCacheKey">The cacheKey after it has been run through fix up.</param>
		/// <returns></returns>
		public TData GetDataFromLongTermCache<TData>(string fixedUpCacheKey, CacheItemMetrics cacheMetrics) where TData : class
		{
			//get it from http LongTerm.
			var item = _longTermCache.Get<TData>(fixedUpCacheKey);

			cacheMetrics.OnItemLongTermCacheLookedUp(item, _metricsUpdater);

			return item;
		}
	}
}
