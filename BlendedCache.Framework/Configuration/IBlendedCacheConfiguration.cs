using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Interface representing the configuration information for blended cache such 
	/// as type loaders and timeouts.  These are the Types that are return from cache, not the types loaded
	/// inside the DataLoader methods.
	/// </summary>
	public interface IBlendedCacheConfiguration
	{
		/// <summary>
		/// Will get the type configuration information for the requested type.  If the EnforceAllLoadedTypesAreDefind flag
		/// is set and the type hasn't been defined, this method will throw a BlendedCacheConfigurationException.
		/// </summary>
		/// <param name="type">The type the configuration object is being requested for.</param>
		/// <returns></returns>
		ITypeConfiguration GetTypeConfiguration(Type type);

		/// <summary>
		/// Contains the default cache timeout structure when a TypeConfiguration is not defined.
		/// </summary>
		ICacheTimeout DefaultCacheTimeout { get; }

		/// <summary>
		/// The default cacheKey converter, these can be overriden through type configuration.
		/// </summary>
		ICacheKeyConverter DefaultCacheKeyConverter { get; }

		/// <summary>
		/// Determines if a prefix should be applied to all cache keys.
		/// </summary>
		string CacheKeyRoot { get; }

		/// <summary>
		/// Will get the cache configuration for the provided type.  Should return the default cacheTimeout when 
		/// there is no type registered.
		/// </summary>
		/// <param name="type">The type to look for the timeouts on.</param>
		/// <returns>Will return the type specific ICacheTimeout or the DefaultCacheTimeout.</returns>
		ICacheTimeout GetCacheTimeoutForTypeOrDefault(Type type);

		//thinking these two methods don't belong on here.  Maybe some IConfigurationResolver
		string GetCacheKeyForTypeOrDefault<TData, TKey>(TKey primaryKey); 
	}
}
