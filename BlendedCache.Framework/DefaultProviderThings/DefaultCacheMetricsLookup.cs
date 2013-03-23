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
	internal class DefaultCacheMetricsLookup<TDataLoader> : ICacheMetricsLookup<TDataLoader>, ICacheMetricsContainer
	{
		/// <summary>
		/// because this is static each TDataLoader this should get it's own dictionary.
		/// </summary>
		private static Dictionary<string, CacheItemMetrics> _lockbox = new Dictionary<string, CacheItemMetrics>();
		private static readonly object _lockBoxInsertLock = new object();

		/// <summary>
		/// Will create a cached item record in the static lock box for recording of metrics of the item.
		/// It could also contain the loader delegate.  If we need to not do a lazy refresh, the loader 
		/// will need to be stored here.
		/// </summary>
		/// <typeparam name="TDataLoader">The type of dataloader the cache key belongs to.</typeparam>
		/// <param name="cacheKey">The cacheKey of the item being retrieved.</param>
		/// <returns>Will never return null.</returns>
		CacheItemMetrics ICacheMetricsLookup<TDataLoader>.GetOrCreateCacheItemMetric(string cacheKey)
		{
			if (_lockbox.ContainsKey(cacheKey))
				return _lockbox[cacheKey];

			lock (_lockBoxInsertLock)
			{
				if (!_lockbox.ContainsKey(cacheKey))
				{
					_lockbox.Add(cacheKey, new CacheItemMetrics(cacheKey));
				}
			}
			return _lockbox[cacheKey];
		}

		/// <summary>
		/// Will get the list of cached items and their metrics.
		/// </summary>
		/// <returns></returns>
		List<Metrics> ICacheMetricsContainer.GetCacheMetrics()
		{
			List<CacheItemMetrics> tempList = null;

			lock (_lockBoxInsertLock)
			{
				tempList = _lockbox.Select(x => x.Value).ToList();
			}

			return tempList.Select(x => x.GetMetrics()).OrderBy(x => x.TimesLoaded).ToList();
		}
	}
}
