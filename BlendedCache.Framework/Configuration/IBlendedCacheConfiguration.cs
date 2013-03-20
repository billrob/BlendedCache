using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Interface representing the configuration information for the defined TDataLoader such 
	/// as type loaders and timeouts.  These are the Types that are return from cache, not the types loaded
	/// inside the TDataLoader methods.
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
		/// Determines if a prefix should be applied to all cache keys.
		/// </summary>
		string CacheKeyRoot { get; }
	}
}
