using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Class for working with the various blended cache metrics under watch.  They are sorted by the TDataLoader underlying them.
	/// </summary>
	public static class BlendedCacheMetricsStore
	{
		private static readonly List<Type> _supportedCaches = new List<Type>();

		/// <summary>
		/// Will register the type as a valid blended cache metrics store.
		/// </summary>
		/// <typeparam name="TDataLoader"></typeparam>
		internal static void RegisterType<TDataLoader>() 
		{
			var type = typeof(TDataLoader);
			if (_supportedCaches.Contains(type))
				return;

			lock (_supportedCaches)
			{
				if (_supportedCaches.Contains(type))
					return;

				_supportedCaches.Add(type);
			}
		}

		/// <summary>
		/// Will get the list of registered types so the caller knows which metrics are currently being stored.
		/// </summary>
		/// <returns></returns>
		public static List<Type> GetRegisteredTypes()
		{
			var list = new List<Type>();

			lock (_supportedCaches)
			{
				foreach (var item in _supportedCaches)
					list.Add(item);
			}

			return list;
		}

		/// <summary>
		/// Will get the metrics for the given data context type.
		/// </summary>
		/// <param name="type">The type to retreive the metrics for.</param>
		public static List<Metrics> GetCacheMetrics(Type type)
		{
			if (!_supportedCaches.Contains(type))
				throw new NotImplementedException("There is not cache metrics for this type: " + type.FullName + ". Perhaps this should return null.");

			var blendedCacheType = typeof(BlendedCache<>).MakeGenericType(type);
			var cache = FormatterServices.GetUninitializedObject(blendedCacheType) as ICacheMetricsContainer;

			if (cache == null)
				throw new NullReferenceException("Not supported, should be BlendedCache<> and ICacheMetricsContainer.");

			return cache.GetCacheMetrics();
		}
	}
}
