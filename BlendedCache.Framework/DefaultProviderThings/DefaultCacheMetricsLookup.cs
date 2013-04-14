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
	internal class DefaultCacheMetricsLookup : ICachedItemMetricsLookup, ICachedItemMetricsContainer
	{
		/// <summary>
		/// All metrics are stored in this list..
		/// </summary>
		private static Dictionary<string, CachedItemMetrics> _lockbox = new Dictionary<string, CachedItemMetrics>(); //cacheKey, Metrics
		private static readonly object _lockBoxInsertLock = new object();

		

		/// <summary>
		/// Will create a cached item record in the static lock box for recording of metrics of the item.
		/// It could also contain the loader delegate.  If we need to not do a lazy refresh, the loader 
		/// will need to be stored here.
		/// </summary>
		/// <param name="cacheKey">The cacheKey of the item being retrieved.</param>
		/// <param name="lookupKey">The lookupKey stored with the item.</param>
		/// <returns>Will never return null.</returns>
		CachedItemMetrics ICachedItemMetricsLookup.GetOrCreateCacheItemMetric<TData, TKey>(string cacheKey, TKey lookupKey)
		{
			if (_lockbox.ContainsKey(cacheKey))
				return _lockbox[cacheKey];

			lock (_lockBoxInsertLock)
			{
				if (!_lockbox.ContainsKey(cacheKey))
				{
					_lockbox.Add(cacheKey, new CachedItemMetrics(cacheKey, lookupKey));
				}
			}
			return _lockbox[cacheKey];
		}

		/// <summary>
		/// Will get the list of cached items and their metrics.
		/// </summary>
		/// <returns></returns>
		List<Metrics> ICachedItemMetricsContainer.GetCacheMetrics()
		{
			List<CachedItemMetrics> tempList = null;

			lock (_lockBoxInsertLock)
			{
				tempList = _lockbox.Select(x => x.Value).ToList();
			}

			return tempList.Select(x => x.GetMetrics()).OrderBy(x => x.TimesLoaded).ToList();
		}

		/// <summary>
		/// Will get a specific cachekey metric.  Very performant compared to get all cache metrics.
		/// </summary>
		/// <param name="cacheKey">The cacheKey to get the metrics for.</param>
		/// <returns></returns>
		Metrics ICachedItemMetricsContainer.GetCachedItemMetrics(string cacheKey)
		{
			if(String.IsNullOrEmpty(cacheKey))
				return null;

			if(_lockbox.ContainsKey(cacheKey))
				return _lockbox[cacheKey].GetMetrics();
			
			return null;
		}

		private ICachedItemMetricsLookup _cacheMetricsLookup
		{
			get { return TryGetService<ICachedItemMetricsLookup>() ?? new DefaultCacheMetricsLookup(); }
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

		private ICacheKeyConverter _cacheKeyConverter
		{
			get { return TryGetService<ICacheKeyConverter>() ?? new DefaultCacheKeyConverter(); }
		}

		#endregion ioc work around
	}
}
