using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Class for working with the various blended cache metrics under watch.
	/// </summary>
	public static class BlendedCacheMetricsStore
	{
		/// <summary>
		/// Will get the metrics from the blended cache store.
		/// </summary>
		public static List<Metrics> GetCacheMetrics()
		{
			return _cachedItemMetricsContainer.GetCacheMetrics();
		}

		/// <summary>
		/// Will get the specific cache metrics object via the lookup key.  Can return null if there are no metrics.
		/// </summary>
		/// <param name="lookupKey">The lookup key to get metrics for. </param>
		/// <returns></returns>
		public static Metrics GetCachedItemMetrics<TData, TKey>(TKey lookupKey) where TData : class
		{
			//todo:0 get the cacheKey root in here somehow.
			var cacheKeyRoot = "";
			var cacheKey = _cacheKeyConverter.ConvertCacheKey<TData, TKey>(cacheKeyRoot, lookupKey);
			return _cachedItemMetricsContainer.GetCachedItemMetrics(cacheKey);
		}

		private static ICachedItemMetricsContainer _cachedItemMetricsContainer
		{
			get { return TryGetService<ICachedItemMetricsContainer>() ?? new DefaultCacheMetricsLookup(); }
		}

		private static ICacheKeyConverter _cacheKeyConverter
		{
			get { return TryGetService<ICacheKeyConverter>() ?? new DefaultCacheKeyConverter(); }
		}

		private static T TryGetService<T>() where T : class
		{
			//todo:3 might need some loving to make this work better with injection.
			return null;
		}
	}
}
