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
	/// Supports DataContract, Serializable by default for ease of use, but can be overridden.
	/// //todo0: enter the process by which people could override this thing.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[DataContract(Namespace = "", Name = "x-bc-entry")]
	[Serializable]
	public class DefaultVolatileCacheEntry<TData> : IVolatileCacheEntry<TData> where TData : class
	{
		private DateTime _expirationDateTimeUtc;

		/// <summary>
		/// Creates a default instance of volatile cache entry.
		/// </summary>
		public DefaultVolatileCacheEntry()
		{

		}

		/// <summary>
		/// Creates a instance of volatile cache entry with the provided cached item and the timeout in seconds.
		/// </summary>
		/// <param name="cachedItem">The item to be cached.</param>
		/// <param name="timeoutInSeconds">The time to be added to UtcNow to determine expiration.</param>
		public DefaultVolatileCacheEntry(TData cachedItem, int timeoutInSeconds)
		{
			CachedItem = cachedItem;
			ExpirationDateTimeUtc = DateTime.UtcNow.AddSeconds(timeoutInSeconds);
		}

		/// <summary>
		/// Will create a instance of default volatile cache entry based on the cachedEntry object.
		/// </summary>
		/// <param name="cachedItem">The item to be cached.</param>
		/// <param name="expirationDateTimeUtc">The expiration date time on the item.</param>
		public DefaultVolatileCacheEntry(TData cachedItem, DateTime expirationDateTimeUtc)
		{
			CachedItem = cachedItem;
			ExpirationDateTimeUtc = expirationDateTimeUtc;
		}

		/// <summary>
		/// The expiration date time in utc of when this volatile cachedItem should expire.
		/// </summary>
		[DataMember(Name="x-bc-expires")]
		public virtual DateTime ExpirationDateTimeUtc
		{
			get { return _expirationDateTimeUtc; }
			set { _expirationDateTimeUtc = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
		}

		/// <summary>
		/// The item to be stored in cache.
		/// </summary>
		[DataMember(Name = "x-bc-item")]
		public virtual TData CachedItem { get; set; }
	}
}
