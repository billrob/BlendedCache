using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// The cache item stored by LongTerm cache.  It is likely the LongTerm cache store is also contains an expiration, which the provider can look at,
	/// however blended cache performs this operation to give a more consistent and predictable experience.
	/// 
	///  todo:github https://github.com/billrob/BlendedCache/issues/26
	/// </summary>
	/// <typeparam name="TData">The type of the cached item.</typeparam>
	public interface ILongTermCacheEntry<TData> where TData : class
	{
		/// <summary>
		/// The item to be stored in cache.
		/// </summary>
		TData CachedItem { get; set; }

		/// <summary>
		/// The date time in utc of when this LongTerm cachedItem should expire.
		/// </summary>
		DateTime ExpirationDateTimeUtc { get; set; }

		/// <summary>
		/// The date time in utc of when this LongTerm cachedItem should be refreshed.  Use expirationDateTimeUtc to 
		/// enforce when it is actually expired and a block needs to occur.
		/// </summary>
		DateTime RefreshDateTimeUtc { get; set; }
	}
}
