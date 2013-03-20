using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Interface class representing the TypeConfiguration structure for a BC managed type.
	/// </summary>
	public interface ITypeConfiguration
	{
		/// <summary>
		/// The SingleLoad delgate for loading a single item of the type specified.  If null, null will be returned out of Cache.Get methods.
		/// </summary>
		object SingleLoad { get; }
		
		/// <summary>
		/// The MultiLoad delegate for loading multiple items of the same type in one call.  Useful for batch load operations as this 
		/// minimizes the caching and database round trips.
		/// </summary>
		object MutliLoad { get; }
		
		/// <summary>
		/// The cache timeout configuration for this type.
		/// </summary>
		ICacheTimeout CacheTimeout { get; }
	}
}
