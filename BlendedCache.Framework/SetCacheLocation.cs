using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Private enum holding instructions of where the data should be set on the cache.
	/// This is used for populating the high layers of cache during a cache miss.  Will flow
	/// to other layers, meaning LongTerm will set all locations. ContextCache will not set Volatile
	/// or LongTerm.
	/// </summary>
	internal enum SetCacheLocation
	{
		/// <summary>
		/// The item will not be set anywhere.
		/// </summary>
		NotSet = 0,

		/// <summary>
		/// The item only be placed in the context cache.
		/// </summary>
		ContextCache = 1,

		/// <summary>
		/// The item will be placed in context and volatile cache.
		/// </summary>
		VolatileCache = 2,

		/// <summary>
		/// The item will be placed in context, volatile, and long term cache.
		/// </summary>
		LongTermCache = 3,
	}
}
