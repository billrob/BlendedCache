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
		public TData Get<TData>(string  lookupKey) where TData : class { return Get<TData, string>(lookupKey);}
		public TData Get<TData>(int lookupKey) where TData : class { return Get<TData, int >(lookupKey); }
		public TData Get<TData, TKey>(TKey lookupKey) where TData : class
		{
			var cacheKey = _cacheKeyConverter.ConvertCacheKey<TData, TKey>(_cacheKeyRoot, lookupKey);

			TData existingItem = null;

			//context cache is the fast one, just return the item.
			if (TryGetDataFromContextCache(cacheKey, out existingItem))
				return existingItem;

			var cacheMetrics = _cacheMetricsLookup.GetOrCreateCacheItemMetric(cacheKey);
			var timeout = _configuration.GetCacheTimeoutForTypeOrDefault(typeof(TData));

			//flushing no need to look further.
			if (_flushMode) return null;

			existingItem = _volatileCacheLookup.GetDataFromVolatileCache<TData>(cacheKey, cacheMetrics);

			//found, so back fill
			if (existingItem != null)
			{
				_cacheSetter.Set(cacheKey, existingItem, timeout, SetCacheLocation.ContextCache, _contextCache, _volatileCache, _longTermCache);
				
				return existingItem;
			}

			//longterm lookup
			existingItem = _longTermCacheLookup.GetDataFromLongTermCache<TData>(cacheKey, cacheMetrics);

			if (existingItem != null)
			{
				_cacheSetter.Set(cacheKey, existingItem, timeout, SetCacheLocation.VolatileCache, _contextCache, _volatileCache, _longTermCache);

				return existingItem;
			}

			//i'm thinking this is where the type loader would go.
			//todo0: add type loader code and finish out flush force load code.

			//found nothing.
			return null;
		}

		public IList<TData> Get<TData>(IList<string> listToLoad) where TData : class
		{
			return Get<TData, string>(listToLoad);
		}
		public IList<TData> Get<TData>(IList<int> listToLoad) where TData : class
		{
			return Get<TData, int>(listToLoad);
		}
		public IList<TData> Get<TData, TKey>(IList<TKey> listToLoad) where TData : class
		{
			var itemsToLookup = new KeyedItemLookupHashSet<TKey, TData>();
			var foundItems = new SortedList<TKey, TData>();
			var timeout = _configuration.GetCacheTimeoutForTypeOrDefault(typeof(TData));

			// create all the lookup keys and get the metrics items
			foreach (var key in listToLoad)
			{
				var cacheKey = _cacheKeyConverter.ConvertCacheKey<TData, TKey>(_cacheKeyRoot, key);
				itemsToLookup.Add(new KeyedItemLookup<TKey, TData>
				{
					CacheKey = cacheKey,
					LookupKey = key,
					Metrics = _cacheMetricsLookup.GetOrCreateCacheItemMetric(cacheKey),
				});
			}

			//grab what we can from the http context
			_contextCacheLookup.SetDataFromContextCache(itemsToLookup, foundItems);

			if (!_flushMode)
			{
				var volatileFound = _volatileCacheLookup.SetDataFromVolatileCache(itemsToLookup, foundItems);
				volatileFound.ForEach(x => _cacheSetter.Set(x.CacheKey, x.CachedItem, timeout, SetCacheLocation.ContextCache, _contextCache, _volatileCache, _longTermCache));

				var longTermFound = _longTermCacheLookup.SetDataFromLongTermCache(itemsToLookup, foundItems);
				longTermFound.ForEach(x => _cacheSetter.Set(x.CacheKey, x.CachedItem, timeout, SetCacheLocation.VolatileCache, _contextCache, _volatileCache, _longTermCache));
			}

			//todo: find everything that's null and reload

			//now build back the list to return in the correct order.
			var returningList = new List<TData>();
			foreach (var key in listToLoad)
			{
				var returningItem = (TData)null;
				foundItems.TryGetValue(key, out returningItem);
				returningList.Add(returningItem);
			}

			return returningList;
		}

		#region Set Method
		public void Set<TData, TKey>(TKey lookupKey, TData cachedItem) where TData : class
		{
			var timeout = _configuration.GetCacheTimeoutForTypeOrDefault(typeof(TData));
			var cacheKey = _cacheKeyConverter.ConvertCacheKey<TData, TKey>(_cacheKeyRoot, lookupKey);

			//will set across the various layers.
			_cacheSetter.Set(cacheKey, cachedItem, timeout, SetCacheLocation.LongTermCache, _contextCache, _volatileCache, _longTermCache);
		}
		#endregion Set Method

		private ICacheKeyConverter _cacheKeyConverter
		{
			get { return TryGetService<ICacheKeyConverter>() ?? new DefaultCacheKeyConverter(); }
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

		private ICacheSetter _cacheSetter
		{
			get { return TryGetService<ICacheSetter>() ?? new DefaultCacheSetter(); }
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

		public TData Get2<TData, TKey>(TKey lookupKey) where TData : class
		{
			return default(TData);
			////so you see not much complexity of code increase.
			////taking the primary key is going to be 
			//var cacheKey = _configuration.GetCacheKeyForTypeOrDefault<TData>(primaryKey);

			//fancy get
			//var item = Get<TData>(cacheKey);

			//if(item != null)
			//	return item;

			//var loader = _configuration.GetLoaderForTypeOrDefault<TData, TKey>();

			//if(loader == null)
			//	return null;

			//item  =loader(dbContext, primaryKey);

			//_cacheSetter.Set(cacheKey, item);

			//return item;
		}
	
	}
}
