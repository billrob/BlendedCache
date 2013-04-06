using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Provides a null volatile cache experience always returning null, and never throwing an exception.
	/// </summary>
	public class NullVolatileCache : IVolatileCache
	{
		/// <summary>
		/// Will do nothing.
		/// </summary>
		void IVolatileCache.Set<TData>(string cacheKey, TData cachedItem, int cacheDurationSeconds)
		{
		}

		/// <summary>
		/// Will always return default(T) which should be null.
		/// </summary>
		TData IVolatileCache.Get<TData>(string cacheKey)
		{
		}

		/// <summary>
		/// Will do nothing.
		/// </summary>
		void IVolatileCache.Remove(string cacheKey)
		{
		}
	}
}
