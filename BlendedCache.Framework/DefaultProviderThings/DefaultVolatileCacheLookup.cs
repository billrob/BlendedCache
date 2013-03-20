using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Implementation the BlendedCache object uses to interact with looking up items in IVolatileCache.
	/// </summary>
	internal class DefaultVolatileCacheLookup : IVolatileCacheLookup
	{
		private readonly IVolatileCache _volatileCache;

		public DefaultVolatileCacheLookup(IVolatileCache volatileCache)
		{
			_volatileCache = volatileCache;
		}

		/// <summary>
		/// Will try to get the specified cacheKey from cache as the provided TData type.
		/// </summary>
		/// <typeparam name="TData"></typeparam>
		/// <param name="fixedUpCacheKey">The cacheKey after it has been run through fix up.</param>
		/// <returns></returns>
		public TData GetDataFromVolatileCache<TData>(string fixedUpCacheKey) where TData : class
		{
			//return it from http context.
			return _volatileCache.Get<TData>(fixedUpCacheKey);
		}
	}
}
