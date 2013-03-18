using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Class used for cleaning/fixing/normalizing cache keys for use with blended cache service.
	/// </summary>
	internal static class CacheKeyHelpers
	{
		/// <summary>
		/// Will clean up the cache key.  eg, TDataContext scoping and ensuring no invalid characters.
		/// </summary>
		/// <param name="originalKey"></param>
		/// <returns></returns>
		internal static string FixUpCacheKey(string cacheKeyRoot, string originalKey)
		{
			if (originalKey.Length > 250)
				throw new NotImplementedException("Bad state here, key can not be more than 250 characters: " + originalKey);

			var stripSpaces = originalKey.Replace(" ", "_");
			if (String.IsNullOrEmpty(cacheKeyRoot))
				return stripSpaces;

			var cacheKey = cacheKeyRoot + stripSpaces;

			if (cacheKey.Length > 250)
				throw new NotImplementedException("Bad state here, key can not be more than 250 characters: " + cacheKey);

			cacheKey = cacheKey.ToLowerInvariant();

			return cacheKey;
		}
	}
}
