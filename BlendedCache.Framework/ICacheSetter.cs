using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Internal interface for actually setting the cached item in the various layers.
	/// </summary>
	internal interface ICacheSetter
	{
		/// <summary>
		/// Will set the cached item across all the various layers.
		/// </summary>
		/// <typeparam name="TData">The type of the object. Normally infered from datatype.</typeparam>
		/// <param name="cacheKey">The cacheKey to be set on the item.</param>
		/// <param name="cachedItem">The cachedItem to be stored.</param>
		/// <param name="timeout">The ICacheTimeout configuration for the item.</param>
		/// <param name="location">The set location.</param>
		void Set<TData>(string cacheKey, TData cachedItem, ICacheTimeout timeout, SetCacheLocation location, IContextCache contextCache, IVolatileCache volatileCache, ILongTermCache longTermCache)
			where TData : class;
	}
}
