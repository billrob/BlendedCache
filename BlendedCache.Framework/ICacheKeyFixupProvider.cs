using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Interface for replacing the ICacheKeyFixupProvider
	/// </summary>
	public interface ICacheKeyFixupProvider
	{
		/// <summary>
		/// Will perform any cleanup operation on the cacheKey before querying all the cache layers.  It's a way to add a namespace to the cache keys.
		/// </summary>
		/// <param name="cacheKeyRoot">The cache key root, comes from BlendedCacheConfiguration</param>
		/// <param name="originalCacheKey">The original cache key from a caller of BlendedCache.</param>
		/// <returns>The cleaned up cache key.</returns>
		string FixUpCacheKey(string cacheKeyRoot, string originalCacheKey);
	}
}
