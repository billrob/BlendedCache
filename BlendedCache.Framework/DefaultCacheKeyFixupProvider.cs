using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Default BlendedCache provider for cleaning up cache keys.
	/// </summary>
	internal class DefaultCacheKeyFixupProvider : ICacheKeyFixupProvider
	{
		/// <summary>
		/// Will perform any cleanup operation on the cacheKey before querying all the cache layers.
		/// </summary>
		/// <param name="cacheKeyRoot">The cache key root, comes from BlendedCacheConfiguration</param>
		/// <param name="originalCacheKey">The original cache key from a caller of BlendedCache.</param>
		/// <returns>The cleaned up cache key.</returns>
		public string FixUpCacheKey(string cacheKeyRoot, string originalCacheKey)
		{
			//todo:3 these validation checks could be moved to config settings if needed.
			if (originalCacheKey.Length > 250)
				throw new NotImplementedException("Bad state here, key can not be more than 250 characters: " + originalCacheKey);

			var stripSpaces = originalCacheKey.Replace(" ", "_");
			if (String.IsNullOrEmpty(cacheKeyRoot))
				return stripSpaces;

			var fixedUpCacheKey = cacheKeyRoot + stripSpaces;

			if (fixedUpCacheKey.Length > 250)
				throw new NotImplementedException("Bad state here, key can not be more than 250 characters: " + fixedUpCacheKey);

			fixedUpCacheKey = fixedUpCacheKey.ToLowerInvariant();

			return fixedUpCacheKey;

		}
	}
}
