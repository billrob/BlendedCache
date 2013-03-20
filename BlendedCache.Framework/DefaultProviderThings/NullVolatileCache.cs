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
		void IVolatileCache.Set<T>(string cacheKey, int cacheDurationSeconds, T data)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Will always return default(T) which should be null.
		/// </summary>
		T IVolatileCache.Get<T>(string cacheKey)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Will do nothing.
		/// </summary>
		void IVolatileCache.Remove(string cacheKey)
		{
			throw new NotImplementedException();
		}
	}
}
