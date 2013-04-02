using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Framework.IntegrationTests
{
	/// <summary>
	/// Holds any helper methods needed by the integration tests.
	/// </summary>
	internal static class TestHelpers
	{
		/// <summary>
		/// Will get a blended cache under the default conditions.
		/// </summary>
		/// <returns></returns>
		public static BlendedCache GetCache(IContextCache contextCache = null, IVolatileCache volatileCache = null, ILongTermCache longTermCache = null, IBlendedCacheConfiguration configuration = null, bool initialFlushMode = false)
		{
			if(contextCache == null)
				contextCache = new DictionaryContextCache();
			if(volatileCache == null)
				volatileCache = new DictionaryVolatileCache();
			if(longTermCache == null)
				longTermCache = new DictionaryLongTermCache();
			if(configuration == null)
				configuration = new BlendedCacheConfiguration();

			var cache = new BlendedCache(contextCache, volatileCache, longTermCache, configuration);

			if (initialFlushMode)
				cache.GetType().GetField("_flushMode", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(cache, true);

			return cache;
		}
	}
}
