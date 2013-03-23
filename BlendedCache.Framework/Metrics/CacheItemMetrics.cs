using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BlendedCache
{
	/// <summary>
	/// This class is written to be internal and never returned outside the BlendedCache assembly because 
	/// of threading concerns.  If this class is needed for another layer, copy the fields onto another object.
	/// Such as the Metrics object.
	/// </summary>
	internal class CacheItemMetrics
	{
		internal CacheItemMetrics(string cacheKey)
		{
			this.CacheKey = cacheKey;
			_dateCreated = DateTime.UtcNow.Ticks;
			this.LoadingLock = new object();
		}

		/// <summary>
		/// The cache key for the item.
		/// </summary>
		internal string CacheKey { get; private set; }

		/// <summary>
		/// Locking object for when this item is loading.
		/// </summary>
		internal object LoadingLock { get; set; }

		/// <summary>
		/// Records when this item was created.
		/// </summary>
		private long _dateCreated;

		/// <summary>
		/// Records when the item when first created in ticks.
		/// </summary>
		private long _firstLoaded;

		/// <summary>
		/// Records when the item was last loaded.
		/// </summary>
		private long _lastLoaded;

		/// <summary>
		/// The number of cache hits against long term cache.
		/// </summary>
		private long _longTermCacheHits;

		/// <summary>
		/// The number of lookups into long term cache for this item.
		/// </summary>
		private long _longTermCacheLookUps;

		/// <summary>
		/// The number of cache misses against long term cache.
		/// </summary>
		private long _longTermCacheMisses;

		/// <summary>
		/// The number of times the cache was flushed/busted.
		/// </summary>
		private long _timesFlushed;

		/// <summary>
		/// The total time spent loading in the background thread.
		/// </summary>
		private long _timeInBackgroundLoad;

		/// <summary>
		/// The total time spent in load.
		/// </summary>
		private long _timeInLoad;

		/// <summary>
		/// The number of times this item failed its background update.
		/// </summary>
		private long _timesBackgroundLoadFailed;

		/// <summary>
		/// The number of times this item has been loaded in a background thread.
		/// </summary>
		private long _timesBackgroundLoaded;

		/// <summary>
		/// The number of of times the remote call executed.
		/// </summary>
		private long _timesLoaded;

		/// <summary>
		/// The number of cache hits against volatile cache.
		/// </summary>
		private long _volatileCacheHits;

		/// <summary>
		/// The number of lookups into volatile cache for this item.
		/// </summary>
		private long _volatileCacheLookUps;

		/// <summary>
		/// The number of cache misses against volatile cache.
		/// </summary>
		private long _volatileCacheMisses;

		/// <summary>
		/// Will update the metrics when an item is loaded by executing delegate.
		/// </summary>
		/// <param name="ticksStart">The ticks start of the loading operation</param>
		/// <param name="ticksEnd">The ticks end of the loading operation.</param>
		/// <param name="flushed">Bool whether the load resulted from a flush.</param>
		internal void OnItemLoaded(long ticksStart, long ticksEnd, bool flushed)
		{
			//never been loaded before.
			Interlocked.CompareExchange(ref _firstLoaded, ticksEnd, 0);

			//update last time remote load occurred. 
			Interlocked.Exchange(ref _lastLoaded, ticksEnd);
			Interlocked.Increment(ref _timesLoaded);
			if (flushed)
				Interlocked.Increment(ref _timesFlushed);

			// this section of code could be protected with a 
			//    if(_timeInLoad != Int64.MaxValue)
			// bill thinks it will normally be false so skip the extra check for performance
			// but if we see lots of items hitting max load time we can put the check in to skip operation

			// currently it will continue overflowing
			unchecked // this is manually set to unchecked to ignore the default build/environment
			{
				//add the load time to the value in load.
				var newValue = Interlocked.Add(ref _timeInLoad, (ticksEnd - ticksStart));

				//when the _timeInLoad overflows, it will flip to be negative
				//set to max value for reporting.
				if (newValue < 0)
					Interlocked.Exchange(ref _timeInLoad, Int64.MaxValue);
			}
		}

		/// <summary>
		/// Will record when the the item was loaded in a background thread.  Passing 0 for ticksStart will result in a failure.
		/// </summary>
		/// <param name="ticksStart">The ticks start of the load operation.</param>
		/// <param name="ticksEnd">The ticks end of the load operation.</param>
		internal void OnItemBackgroundUpdated(long ticksStart, long ticksEnd)
		{
			Interlocked.Increment(ref _timesBackgroundLoaded);

			if (ticksStart == 0)
			{
				Interlocked.Increment(ref _timesBackgroundLoadFailed);
				return;
			}

			//update last time remote load occurred. 
			Interlocked.Exchange(ref _lastLoaded, ticksEnd);

			// currently it will continue overflowing
			unchecked // this is manually set to unchecked to ignore the default build/environment
			{
				//add the load time to the value in load.
				var newValue = Interlocked.Add(ref _timeInBackgroundLoad, (ticksEnd - ticksStart));

				//when the _timeInLoad overflows, it will flip to be negative
				//set to max value for reporting.
				if (newValue < 0)
					Interlocked.Exchange(ref _timeInBackgroundLoad, Int64.MaxValue);
			}
		}

		/// <summary>
		/// Will update the metrics when an item is looked up in volatile cache.
		/// </summary>
		/// <param name="item">The item that was loaded from cache.  Null implies a miss.</param>
		/// <param name="webRequestMetrics">The web request object for tracking thread performance.</param>
		internal void OnItemVolatileCacheLookedUp(object item, IWebRequestCacheMetricsUpdater metricsUpdator)
		{
			metricsUpdator.Increment_Cache_VolatileLookup();
			Interlocked.Increment(ref _volatileCacheLookUps);

			if (item == null)
			{
				metricsUpdator.Increment_Cache_VolatileMisses();
				Interlocked.Increment(ref _volatileCacheMisses);
			}
			else
			{
				metricsUpdator.Increment_Cache_VolatileHits();
				Interlocked.Increment(ref _volatileCacheHits);
			}
		}

		/// <summary>
		/// Will update the metrics when an item is looked up in long term cache.
		/// </summary>
		/// <param name="item">The item that was loaded from cache.  Null implies a miss.</param>
		/// <param name="webRequestMetrics">The web request object for tracking thread performance.</param>
		internal void OnItemLongTermCacheLookedUp(object item, IWebRequestCacheMetricsUpdater metricsUpdator)
		{
			metricsUpdator.Increment_Cache_LongTermLookup();
			Interlocked.Increment(ref _longTermCacheLookUps);

			if (item == null)
			{
				metricsUpdator.Increment_Cache_LongTermMisses();
				Interlocked.Increment(ref _longTermCacheMisses);
			}
			else
			{
				metricsUpdator.Increment_Cache_LongTermHits();
				Interlocked.Increment(ref _longTermCacheHits);
			}
		}

		internal Metrics GetMetrics()
		{
			return new Metrics()
			{
				CacheKey = this.CacheKey,
				DateCreated = new DateTime(this._dateCreated),
				FirstLoaded = new DateTime(this._firstLoaded),
				LastLoaded = new DateTime(this._lastLoaded),
				LongTermCacheHits = this._longTermCacheHits,
				LongTermCacheLookUps = this._longTermCacheLookUps,
				LongTermCacheMisses = this._longTermCacheMisses,
				TimeInBackgroundLoad = this._timeInBackgroundLoad,
				TimeInLoad = this._timeInLoad,
				TimesBackgroundLoaded = this._timesBackgroundLoaded,
				TimesBackgroundLoadFailed = this._timesBackgroundLoadFailed,
				TimesFlushed = this._timesFlushed,
				TimesLoaded = this._timesLoaded,
				VolatileCacheHits = this._volatileCacheHits,
				VolatileCacheLookUps = this._volatileCacheLookUps,
				VolatileCacheMisses = this._volatileCacheMisses,
			};
		}
	}
}
