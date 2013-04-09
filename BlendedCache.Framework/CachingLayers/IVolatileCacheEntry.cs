using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// The cache item stored by volatile cache.  It is likely the volatile cache store is also contains an expiration, which the provider can look at,
	/// however blended cache performs this operation to give a more consistent and predictable experience.
	/// 
	///  //todo0: enter the process by which people could override this thing.
	/// </summary>
	/// <typeparam name="TData">The type of the cached item.</typeparam>
	public interface IVolatileCacheEntry<TData> where TData : class
	{
		/// <summary>
		/// The item to be stored in cache.
		/// </summary>
		TData CachedItem { get; set; }

		/// <summary>
		/// The expiration date time in utc of when this volatile cachedItem should expire.
		/// </summary>
		DateTime ExpirationDateTimeUtc { get; set; }
	}
}
