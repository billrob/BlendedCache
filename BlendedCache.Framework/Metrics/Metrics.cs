using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// The public version of the metrics
	/// </summary>
	public class Metrics
	{
		/// <summary>
		/// The lookup key for the item.
		/// </summary>
		public object LookupKey { get; internal set; }

		/// <summary>
		/// The cache key for the item.
		/// </summary>
		public string CacheKey { get; internal set; }

		/// <summary>
		/// Records when this item was created.
		/// </summary>
		public DateTime DateCreated { get; internal set; }

		/// <summary>
		/// Records when the item when first created.
		/// </summary>
		public DateTime FirstLoaded { get; internal set; }

		/// <summary>
		/// Records when the item was last loaded.
		/// </summary>
		public DateTime LastLoaded { get; internal set; }

		/// <summary>
		/// The number of cache hits against long term cache.
		/// </summary>
		public long LongTermCacheHits { get; internal set; }

		/// <summary>
		/// The number of lookups into long term cache for this item.
		/// </summary>
		public long LongTermCacheLookUps { get; internal set; }

		/// <summary>
		/// The number of cache misses against long term cache.
		/// </summary>
		public long LongTermCacheMisses { get; internal set; }

		/// <summary>
		/// The number of times the cache was flushed/busted.
		/// </summary>
		public long TimesFlushed { get; internal set; }

		/// <summary>
		/// The total time spent loading in the background thread.
		/// </summary>
		public long TimeInBackgroundLoad { get; internal set; }

		/// <summary>
		/// The total time spent in load.
		/// </summary>
		public long TimeInLoad { get; internal set; }

		/// <summary>
		/// The number of times this item failed its background update.
		/// </summary>
		public long TimesBackgroundLoadFailed { get; internal set; }

		/// <summary>
		/// The number of times this item has been loaded in a background thread.
		/// </summary>
		public long TimesBackgroundLoaded { get; internal set; }

		/// <summary>
		/// The number of of times the remote call executed.
		/// </summary>
		public long TimesLoaded { get; internal set; }

		/// <summary>
		/// The number of cache hits against volatile cache.
		/// </summary>
		public long VolatileCacheHits { get; internal set; }

		/// <summary>
		/// The number of lookups into volatile cache for this item.
		/// </summary>
		public long VolatileCacheLookUps { get; internal set; }

		/// <summary>
		/// The number of cache misses against volatile cache.
		/// </summary>
		public long VolatileCacheMisses { get; internal set; }
	}
}
