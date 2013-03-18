﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Interface controlling the timeout behavior of the various caching blocks.
	/// </summary>
	public interface ICacheTimeout
	{
		/// <summary>
		/// The time when the item should expire from volatile cache.
		/// </summary>
		int VolatileTimeoutInSeconds { get; }

		/// <summary>
		/// The time when the item should expire from long term cache and stale data should not be returned.
		/// Unless the database is down.
		/// </summary>
		int LongTermTimeoutInSeconds { get; }

		/// <summary>
		/// The time in seconds when blended cache should force a refresh of this cache item and serve out
		/// potentially stale data during that time.
		/// </summary>
		int LongTermRefreshInSeconds { get; }
	}
}
