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
	public interface ICacheKeyConverter
	{
		/// <summary>
		/// Will convert the lookupKey to a cacheKey to storing inside cache.
		/// </summary>
		/// <param name="cacheKeyRoot">The cache key root, comes from BlendedCacheConfiguration.</param>
		/// <param name="lookupKey">The original lookupKey from a caller of BlendedCache.</param>
		/// <returns>The produced cache key.</returns>
		string ConvertCacheKey<TData, TKey>(string cacheKeyRoot, TKey lookupKey);
	}
}
