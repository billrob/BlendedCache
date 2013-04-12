using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Default BlendedCache provider for converting cacheKeys
	/// </summary>
	public class DefaultCacheKeyConverter : ICacheKeyConverter
	{
		/// <summary>
		/// Will convert the lookupKey to a cacheKey to storing inside cache.
		/// </summary>
		/// <param name="cacheKeyRoot">The cache key root, comes from BlendedCacheConfiguration.</param>
		/// <param name="lookupKey">The original lookupKey from a caller of BlendedCache.</param>
		/// <returns>The produced cache key.</returns>
		public virtual string ConvertCacheKey<TData, TKey>(string cacheKeyRoot, TKey lookupKey)
		{
			//this could also look for a simple interface, or a by convention method such as ToCacheKey() for complex request objects
			var initialCacheKey = typeof(TData).FullName + "." + lookupKey.ToString();

			//todo:3 these validation checks could be moved to config settings if needed.
			if (initialCacheKey.Length > 250)
				throw new NotImplementedException("Bad state here, key can not be more than 250 characters: " + initialCacheKey);

			var stripSpaces = initialCacheKey.Replace(" ", "_");
			if (String.IsNullOrEmpty(cacheKeyRoot))
				return stripSpaces;

			var fixedUpCacheKey = cacheKeyRoot + stripSpaces;

			if (fixedUpCacheKey.Length > 250)
				throw new NotImplementedException("Bad state here, key can not be more than 250 characters: " + fixedUpCacheKey); //memcachedLimitation, although we could hash it

			fixedUpCacheKey = fixedUpCacheKey.ToLowerInvariant();

			return fixedUpCacheKey;
		}

		/// <summary>
		/// Will get a converter than means the lookupKey is also the cacheKey.
		/// </summary>
		public static ICacheKeyConverter MyLookupKeyIsTheCacheKey
		{
			get { return new MyLookupKeyIsTheCacheKeyConverter(); }
		}

		private class MyLookupKeyIsTheCacheKeyConverter : ICacheKeyConverter
		{
			public string ConvertCacheKey<TData, TKey>(string cacheKeyRoot, TKey lookupKey)
			{
				if (String.IsNullOrEmpty(cacheKeyRoot))
					return lookupKey.ToString(); // this might still need to run through some sort of space or non-ascii char parser.

				return cacheKeyRoot + "." + lookupKey.ToString();
			}
		}
	}
}