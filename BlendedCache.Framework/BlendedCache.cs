using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// The blended cache container for working with Context, Volatile, and LongTerm cache with fancy single 
	/// and multi-key load methods.
	/// </summary>
	/// <typeparam name="TDataLoader">Typically a dataContext that is passed to the delegate if provided.</typeparam>
	public class BlendedCache<TDataLoader> : IBlendedCache<TDataLoader>, ICacheMetricsContainer
	{
		private bool _flushMode;
		private readonly IContextCache _contextCache;
		private Logging.ILogger Logger;
		
		public BlendedCache(IContextCache contextCache)
		{
			_contextCache = contextCache;

			// create a logger for this class
			this.Logger = Logging.LoggerHelper.GetLogger(this.GetType());
		}

		/// <summary>
		/// Will set the current flush mode on this service.  It returns the current state of the value.
		/// </summary>
		public bool SetFlushMode(bool flushMode)
		{
			bool current = _flushMode;

			Logger.Info("Flush mode enabled");

			//reset the context caching when the flush mode is being turned ON.
			if (!current && flushMode)
			{
				//todo:2 should this move to a .Clear() method?
				foreach (var key in _contextCache.Keys)
				{
					_contextCache.Remove(key);
				}
			}

			_flushMode = flushMode;

			return current;
		}

		List<Metrics> ICacheMetricsContainer.GetCacheMetrics()
		{
			//when changing, check the MetricsStoreTests.
			return new List<Metrics>();
		}
	}
}
