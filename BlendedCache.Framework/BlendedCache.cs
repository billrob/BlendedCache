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
	public class BlendedCache : IBlendedCache
	{
		private bool _flushMode;
		private readonly IContextCache _contextCache;
		private readonly IVolatileCache _volatileCache;
		private readonly ILongTermCache _longTermCache;
		private readonly IBlendedCacheConfiguration _configuration;
		private readonly string _cacheKeyRoot;
		private readonly Logging.ILogger _logger;


		public BlendedCache(IContextCache contextCache, IVolatileCache volatileCache, ILongTermCache longTermCache, IBlendedCacheConfiguration configuration)
		{
			_contextCache = contextCache ?? new NullContextCache();
			_volatileCache = volatileCache ?? new NullVolatileCache();
			_longTermCache = longTermCache ?? new NullLongTermCache();
			_configuration = configuration ?? new BlendedCacheConfiguration();
			_cacheKeyRoot = _configuration.CacheKeyRoot; //optimized because it is looked up so many times.

			// create a logger for this class
			this._logger = Logging.LoggerHelper.GetLogger(this.GetType());
		}

		/// <summary>
		/// Will turn on/off flush mode for the cache if not already activated.  When turned on initially 
		/// the ContextCache is emptied but will cache further get calls to ensure data consistency through a 
		/// give code path.  LongTerm and Volatile will always return null when flush mode is activated.  Use sparingly
		/// </summary>
		/// <returns>The previous state of flushMode after setting it.</returns>
		public bool SetFlushMode(bool flushMode)
		{
			bool current = _flushMode;

			_logger.Info("Flush mode enabled");

			//reset the context caching when the flush mode is being turned ON.
			if (!current && flushMode)
				_contextCache.Clear();

			_flushMode = flushMode;

			return current;
		}

		/// <summary>
		/// Will get the specified item based on the cacheKey.
		/// </summary>
		/// <typeparam name="TData">The type of data that should be returned.</typeparam>
		/// <param name="cacheKey">The cacheKey of the item to be retrieved.</param>
		/// <returns>The item requests or null.  If TypeConfigurations are registered, the DataLoader will be executed.</returns>
		public TData Get<TData>(string cacheKey) where TData : class
		{
			var fixedUpCacheKey = _cacheKeyFixupProvider.FixUpCacheKey(_cacheKeyRoot, cacheKey);

			TData existingItem = null;

			//context cache is the fast one, just return the item.
			if (TryGetDataFromContextCache(fixedUpCacheKey, out existingItem))
				return existingItem;

			var cacheMetrics = _cacheMetricsLookup.GetOrCreateCacheItemMetric(fixedUpCacheKey);

			//flushing no need to look further.
			if (_flushMode) return null;

			existingItem = _volatileCacheLookup.GetDataFromVolatileCache<TData>(fixedUpCacheKey, cacheMetrics);

			//longterm lookup
			if (existingItem == null)
				existingItem = _longTermCacheLookup.GetDataFromLongTermCache<TData>(fixedUpCacheKey, cacheMetrics);

			return existingItem;
		}


		#region Set Method
		public void Set<TData>(string cacheKey, TData cachedItem) where TData : class
		{
			var timeout = _configuration.GetCacheTimeoutForTypeOrDefault(typeof(TData));

			//will set across the various layers.
			_cacheSetter.Set(cacheKey, cachedItem, timeout, SetCacheLocation.LongTermCache, _contextCache, _volatileCache, _longTermCache);
		}
		#endregion Set Method

		private ICacheKeyFixupProvider _cacheKeyFixupProvider
		{
			get { return TryGetService<ICacheKeyFixupProvider>() ?? new DefaultCacheKeyFixupProvider(); }
		}

		private IContextCacheLookup _contextCacheLookup
		{
			get { return TryGetService<IContextCacheLookup>() ?? new DefaultContextCacheLookup(_contextCache); }
		}

		private IVolatileCacheLookup _volatileCacheLookup
		{
			get { return TryGetService<IVolatileCacheLookup>() ?? new DefaultVolatileCacheLookup(_volatileCache, _metricsUpdater); }
		}

		private ILongTermCacheLookup _longTermCacheLookup
		{
			get { return TryGetService<ILongTermCacheLookup>() ?? new DefaultLongTermCacheLookup(_longTermCache, _metricsUpdater); }
		}

		private ICacheMetricsLookup _cacheMetricsLookup
		{
			get { return TryGetService<ICacheMetricsLookup>() ?? new DefaultCacheMetricsLookup(); }
		}

		private IWebRequestCacheMetricsUpdater _metricsUpdater
		{
			get { return TryGetService<IWebRequestCacheMetricsUpdater>() ?? new NullWebRequestCacheMetricsUpdater(); }
		}

		private IBlendedCacheSetter _cacheSetter
		{
			get { return TryGetService<IBlendedCacheSetter>() ?? new DefaultBlendedCacheSetter(); }
		}










		/// <summary>
		/// Will call into the context cache lookup and return true or false if the out parameter was populated.
		/// </summary>
		/// <typeparam name="TData">The TData of the object to load.</typeparam>
		/// <param name="fixedUpCacheKey">The fixed up cache key.</param>
		/// <param name="existingItem">The out parameter that can contain the existing item.</param>
		/// <returns></returns>
		private bool TryGetDataFromContextCache<TData>(string fixedUpCacheKey, out TData existingItem) where TData : class
		{
			existingItem = _contextCacheLookup.GetDataFromContextCache<TData>(fixedUpCacheKey);

			return existingItem != null;
		}
















		#region ioc work around
		private readonly Dictionary<Type, object> _iocBag = new Dictionary<Type, object>();

		/// <summary>
		/// Will get a service from the ioc container or the injected IoC bag.  Typically for tests, they will
		/// be injected with a SetService call.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		internal T GetService<T>() where T : class
		{
			if (_iocBag.ContainsKey(typeof(T)))
				return (T)_iocBag[typeof(T)];

			throw new ArgumentNullException("There is no ioc registration or injection for type: " + typeof(T) + " for class: " + this.GetType().Name);
		}

		/// <summary>
		/// Will get a service from the ioc container or the injected IoC bag.  Typically for tests, they will
		/// be injected with a SetService call.
		/// Will return null if it doesn't exist.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		internal T TryGetService<T>() where T : class
		{
			if (_iocBag.ContainsKey(typeof(T)))
				return (T)_iocBag[typeof(T)];

			return null;
		}

		/// <summary>
		/// Will set the service into the ioc bag.  Useful for testing.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="service"></param>
		internal void SetService<T>(T service)
		{
			_iocBag[typeof(T)] = service;
		}

		#endregion ioc work around
	}
}
