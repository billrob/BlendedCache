using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Iterface object for working with the cache
	/// </summary>
	public interface IWebRequestCacheMetricsUpdater
	{
		/// <summary>
		/// Will increment the cache long term hits (found) count for this request.
		/// </summary>
		void Increment_Cache_LongTermHits();
		
		/// <summary>
		/// Will increment the cache long term lookup count for this request.
		/// </summary>
		void Increment_Cache_LongTermLookup();
		
		/// <summary>
		/// Will increment the cache long term miss (not found) count for this request.
		/// </summary>
		void Increment_Cache_LongTermMisses();

		/// <summary>
		/// Will increment the cache volatile hits (found) count for this request.
		/// </summary>
		void Increment_Cache_VolatileHits();

		/// <summary>
		/// Will increment the cache volatile lookup count for this request.
		/// </summary>
		void Increment_Cache_VolatileLookup();

		/// <summary>
		/// Will increment the cache volatile miss (not found) count for this request.
		/// </summary>
		void Increment_Cache_VolatileMisses();
	}
}
