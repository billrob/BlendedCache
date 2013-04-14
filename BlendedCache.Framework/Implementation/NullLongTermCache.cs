using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Provides a null longTerm cache experience always returning null, and never throwing an exception.
	/// </summary>
	public class NullLongTermCache : ILongTermCache
	{
		/// <summary>
		/// Will always return default(T) which should be null.
		/// </summary>
		ILongTermCacheEntry<TData> ILongTermCache.Get<TData>(string cacheKey)
		{
			return null;
		}
		
		/// <summary>
		/// Will do nothing.
		/// </summary>
		void ILongTermCache.Set<TData>(string cacheKey, ILongTermCacheEntry<TData> cachedItem)
		{
			return;
		}

		/// <summary>
		/// Will return an empty dictionary.
		/// </summary>
		public IDictionary<string, ILongTermCacheEntry<TData>> Get<TData>(IEnumerable<string> cacheKeys) where TData : class
		{
			return new Dictionary<string, ILongTermCacheEntry<TData>>();
		}

		/// <summary>
		/// Will return a safely callable, but does nothing ILongTermCache implementation.
		/// </summary>
		public static ILongTermCache NullInstance
		{
			get
			{
				if (_nullInstance == null) // not worried about thread safety here and it will eventually resolve.
					_nullInstance = new NullLongTermCache();

				return _nullInstance;
			}
		}
		private static ILongTermCache _nullInstance;


		
	}
}
