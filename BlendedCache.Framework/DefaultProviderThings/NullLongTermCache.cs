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
		T ILongTermCache.Get<T>(string cacheKey)
		{
			return null;
		}
		void ILongTermCache.Set<TData>(string cacheKey, TData cachedItem, int refreshSeconds, int absoluteExpirationSeconds)
		{
			return;
		}
	}
}
