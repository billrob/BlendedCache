﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// The cache metrics lookup object.
	/// </summary>
	internal class CacheMetricsLookup<TDataLoader> : ICacheMetricsLookup
	{
		/// <summary>
		/// Will create a cached item record in the static lock box for recording of metrics of the item.
		/// It could also contain the loader delegate.  If we need to not do a lazy refresh, the loader 
		/// will need to be stored here.
		/// </summary>
		/// <typeparam name="TDataLoader">The type of dataloader the cache key belongs to.</typeparam>
		/// <param name="cacheKey">The cacheKey of the item being retrieved.</param>
		/// <returns>Will never return null.</returns>
		CacheItemMetrics ICacheMetricsLookup.GetOrCreateCacheItemMetric(string cacheKey)
		{
			return null;
		}
	}
}
