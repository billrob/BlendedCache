using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Implementation the BlendedCache object uses to interact with looking up items in IContextCache.
	/// </summary>
	internal class DefaultContextCacheLookup : IContextCacheLookup
	{
		private readonly IContextCache _contextCache;

		public DefaultContextCacheLookup(IContextCache contextCache)
		{
			_contextCache = contextCache;
		}

		/// <summary>
		/// Will try to get the specified cacheKey from cache as the provided TData type.
		/// </summary>
		/// <typeparam name="TData"></typeparam>
		/// <param name="fixedUpCacheKey">The cacheKey after it has been run through fix up.</param>
		/// <returns></returns>
		public TData GetDataFromContextCache<TData>(string fixedUpCacheKey) where TData : class
		{
			//return it from http context.
			return _contextCache.Get<TData>(fixedUpCacheKey);
		}
	}
}
