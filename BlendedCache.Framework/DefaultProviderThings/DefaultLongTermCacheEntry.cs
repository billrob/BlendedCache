using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// The cache item stored by LongTerm cache.  It is likely the LongTerm cache store is also contains an expiration, which the provider can look at,
	/// however blended cache performs this operation to give a more consistent and predictable experience.
	/// 
	/// Supports DataContract, Serializable by default for ease of use, but can be overridden.
	/// //todo0: enter the process by which people could override this thing.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[DataContract(Namespace = "", Name = "x-bc-entry")]
	[Serializable]
	public class DefaultLongTermCacheEntry<TData> : ILongTermCacheEntry<TData> where TData : class
	{
		private DateTime _expirationDateTimeUtc;
		private DateTime _refreshDateTimeUtc;

		/// <summary>
		/// Creates a default instance of LongTerm cache entry.
		/// </summary>
		public DefaultLongTermCacheEntry()
		{

		}

		/// <summary>
		/// Creates a instance of LongTerm cache entry with the provided cached item and the timeout in seconds.
		/// </summary>
		/// <param name="cachedItem">The item to be cached.</param>
		/// <param name="timeoutInSeconds">The time to be added to UtcNow to determine expiration.</param>
		/// <param name="refreshInSeconds">The time to be added to UtcNow to determine refresh.</param>
		public DefaultLongTermCacheEntry(TData cachedItem, int timeoutInSeconds, int refreshInSeconds)
		{
			CachedItem = cachedItem;
			var now = DateTime.UtcNow;
			ExpirationDateTimeUtc = now.AddSeconds(timeoutInSeconds);
			RefreshDateTimeUtc = now.AddSeconds(refreshInSeconds);
		}

		/// <summary>
		/// Will create a instance of default LongTerm cache entry based on the cachedEntry object.
		/// </summary>
		/// <param name="cachedItem">The item to be cached.</param>
		/// <param name="expirationDateTimeUtc">The expiration date time on the item.</param>
		/// <param name="refreshDateTimeUtc">The refresh date time on the item.</param>
		public DefaultLongTermCacheEntry(TData cachedItem, DateTime expirationDateTimeUtc, DateTime refreshDateTimeUtc)
		{
			CachedItem = cachedItem;
			ExpirationDateTimeUtc = expirationDateTimeUtc;
			RefreshDateTimeUtc = refreshDateTimeUtc;
		}

		/// <summary>
		/// The expiration date time in utc of when this LongTerm cachedItem should expire.
		/// </summary>
		[DataMember(Name="x-bc-expires")]
		public virtual DateTime ExpirationDateTimeUtc
		{
			get { return _expirationDateTimeUtc; }
			set { _expirationDateTimeUtc = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
		}

		/// <summary>
		/// The date time in utc of when this LongTerm cachedItem should be refreshed.
		/// </summary>
		[DataMember(Name = "x-bc-refreshes")]
		public virtual DateTime RefreshDateTimeUtc
		{
			get { return _refreshDateTimeUtc; }
			set { _refreshDateTimeUtc = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
		}
		
		/// <summary>
		/// The item to be stored in cache.
		/// </summary>
		[DataMember(Name = "x-bc-item")]
		public virtual TData CachedItem { get; set; }
	}
}
