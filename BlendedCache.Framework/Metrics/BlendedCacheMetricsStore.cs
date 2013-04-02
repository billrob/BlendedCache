﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Class for working with the various blended cache metrics under watch.  They are sorted by the DataLoader underlying them.
	/// </summary>
	public static class BlendedCacheMetricsStore
	{
		/// <summary>
		/// Will get the metrics from the blended cache store.
		/// </summary>
		public static List<Metrics> GetCacheMetrics()
		{
			//todo:3 might need some loving to make this work better with injection.
			var store = new DefaultCacheMetricsLookup() as ICacheMetricsContainer;

			if (store == null)
				throw new NotImplementedException("There is no cache metrics, maybe this should be returning null or empty list instead.");

			return store.GetCacheMetrics();
		}
	}
}
