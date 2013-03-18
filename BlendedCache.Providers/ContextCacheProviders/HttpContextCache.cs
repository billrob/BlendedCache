using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BlendedCache.Providers
{
	/// <summary>
	/// Wraps HttpContext.Current.Items collecion.  Has null safety built so this object will degrade gracefully if used in 
	/// background thread operations.
	/// </summary>
	public class HttpContextCache : IContextCache
	{
		//todo:0 pull over final docs.
		void IContextCache.Set<T>(string key, T value)
		{
			if (null != HttpContext.Current)
			{
				key = MakeCacheKeyForContextCaching(key);
				HttpContext.Current.Items[key] = value;
			}
		}

		T IContextCache.Get<T>(string key)
		{
			if (null == HttpContext.Current)
				return default(T);

			key = MakeCacheKeyForContextCaching(key);

			return (T)HttpContext.Current.Items[key];
		}

		IEnumerable<string> IContextCache.Keys
		{
			get
			{
				if (null == HttpContext.Current)
					return Enumerable.Empty<string>();

				var matchingKeys = HttpContext.Current.Items.Keys.OfType<string>()
					.Where(x => x.StartsWith(ContextCacheKeyPrefix));
				return matchingKeys.Select(x => x.Substring(ContextCacheKeyPrefix.Length));
			}
		}

		void IContextCache.Remove(string key)
		{
			key = MakeCacheKeyForContextCaching(key);

			if (null != HttpContext.Current)
			{
				HttpContext.Current.Items.Remove(key);
			}
		}

		private const string ContextCacheKeyPrefix = "x-BC-";

		/// <summary>
		/// Converts the normalized cache key into something safe to be placed into a shared context cache.
		/// </summary>
		/// <param name="cacheKey">The cacheKey to add the prefix to.</param>
		/// <returns></returns>
		private static string MakeCacheKeyForContextCaching(string cacheKey)
		{
			return ContextCacheKeyPrefix + cacheKey;
		}

	}

}
