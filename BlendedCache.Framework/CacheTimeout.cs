using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Default class implementing the cache timeout.
	/// </summary>
	public class CacheTimeout : ICacheTimeout
	{
		/// <summary>
		/// Will create a default cache timeout block, 60 volatile, 5 minute long term refresh, 1 hour long term absolute
		/// </summary>
		public CacheTimeout()
		{
			VolatileTimeoutInSeconds = 60;
			LongTermRefreshInSeconds = 5 * 60;
			LongTermTimeoutInSeconds = 1 * 60 * 60;

		}
		/// <summary>
		/// The time when the item should expire from volatile cache.
		/// </summary>
		public int VolatileTimeoutInSeconds { get; set; }

		/// <summary>
		/// The time when the item should expire from long term cache and stale data should not be returned.
		/// Unless the database is down.
		/// </summary>
		public int LongTermTimeoutInSeconds { get; set; }

		/// <summary>
		/// The time in seconds when blended cache should force a refresh of this cache item and serve out
		/// potentially stale data during that time.
		/// </summary>
		public int LongTermRefreshInSeconds { get; set; }
	}
}
