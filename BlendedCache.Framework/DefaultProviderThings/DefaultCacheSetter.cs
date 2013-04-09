using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	internal class DefaultCacheSetter : ICacheSetter
	{
		void ICacheSetter.Set<TData>(string cacheKey, TData cachedItem, ICacheTimeout timeout, SetCacheLocation location, IContextCache contextCache, IVolatileCache volatileCache, ILongTermCache longTermCache)
		{
			var volatileCacheEntry = (DefaultVolatileCacheEntry<TData>)null;

			//man, I miss fall through switch statements.
			switch (location)
			{
				case SetCacheLocation.LongTermCache:
					//longTermCache.Set(cacheKey, cachedItem, timeout.LongTermRefreshInSeconds, timeout.LongTermTimeoutInSeconds);
					volatileCacheEntry = new DefaultVolatileCacheEntry<TData>(cachedItem, timeout.VolatileTimeoutInSeconds);
					break;
				case SetCacheLocation.VolatileCache:
					volatileCacheEntry = new DefaultVolatileCacheEntry<TData>(cachedItem, timeout.VolatileTimeoutInSeconds);
					break;
				default:
					break; // do nothing
			}

			//man, I miss fall through switch statements.
			switch (location)
			{
				case SetCacheLocation.LongTermCache:
					longTermCache.Set(cacheKey, cachedItem, timeout.LongTermRefreshInSeconds, timeout.LongTermTimeoutInSeconds);
					volatileCache.Set(cacheKey, volatileCacheEntry);
					contextCache.Set(cacheKey, cachedItem);
					break;
				case SetCacheLocation.VolatileCache:
					volatileCache.Set(cacheKey, volatileCacheEntry);
					contextCache.Set(cacheKey, cachedItem);
					break;
				case SetCacheLocation.ContextCache:
					contextCache.Set(cacheKey, cachedItem);
					break;
				default:
					break; // do nothing
			}

		}
	}
}