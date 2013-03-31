using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	internal class DefaultBlendedCacheSetter : IBlendedCacheSetter
	{
		void IBlendedCacheSetter.Set<TData>(string cacheKey, TData cachedItem, ICacheTimeout timeout, SetCacheLocation location, IContextCache contextCache, IVolatileCache volatileCache, ILongTermCache longTermCache)
		{
			//man, I miss fall through switch statements.
			switch (location)
			{
				case SetCacheLocation.LongTermCache:
					longTermCache.Set(cacheKey, cachedItem, timeout.LongTermRefreshInSeconds, timeout.LongTermTimeoutInSeconds);
					volatileCache.Set(cacheKey, cachedItem, timeout.VolatileTimeoutInSeconds);
					contextCache.Set(cacheKey, cachedItem);
					break;
				case SetCacheLocation.VolatileCache:
					volatileCache.Set(cacheKey, cachedItem, timeout.VolatileTimeoutInSeconds);
					contextCache.Set(cacheKey, cachedItem);
					break;
				case SetCacheLocation.ContextCache:
					contextCache.Set(cacheKey, cachedItem);
					break;
				default:
					return; // do nothing
			}
			
		}
	}
}
