using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// The long term cache interface.  Typically this will be in memcached, app fabric, or some other 
	/// distributed out of process caching.
	/// </summary>
	public interface ILongTermCache
	{
		/// <summary>
		/// Will get a strongly typed object from longterm cache.  Will return null if the item does not exist.
		/// </summary>
		/// <typeparam name="TData">The type of the object. Normally infered from datatype.</typeparam>
		/// <param name="cacheKey">The cache key of the cached item.</param>
		/// <returns>The entry stored in LongTermCache, not checked for any expiration.</returns>
		ILongTermCacheEntry<TData> Get<TData>(string cacheKey) where TData : class;

		/// <summary>
		/// Will get a list of stronly typed objects from long term cache. Return return a null entry, but the key will be there when does not exist.
		/// </summary>
		/// <typeparam name="TData"></typeparam>
		/// <param name="cacheKeys"></param>
		/// <returns></returns>
		IDictionary<string, ILongTermCacheEntry<TData>> Get<TData>(IEnumerable<string> cacheKeys) where TData : class;

		/// <summary>
		/// Will set the cachedItem under the cacheKey using the specified refresh rules.
		/// </summary>
		/// <typeparam name="TData">The type of the object. Normally infered from datatype.</typeparam>
		/// <param name="cacheKey">The cacheKey for the item.</param>
		/// <param name="cachedItem">The actual item to be stored in volatile cache.</param>
		/// <param name="refreshSeconds">The duration in seconds until this item is eligible for refresh.  The cachedItem is still 
		/// considered valid.  Use absoluteExpiration to actually expire.</param>
		/// <param name="absoluteExpirationSeconds">The duration in seconds until this item is considred expired.  
		/// Because it is recommended to never expire items from the long term cache, this control that null behavior.</param>
		void Set<TData>(string cacheKey, ILongTermCacheEntry<TData> cachedItem) where TData : class;
	}
}
