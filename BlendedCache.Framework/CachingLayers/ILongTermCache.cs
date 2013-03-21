using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// The long term cache interface.  Typically this will be in memcached, app fabric, or some other 
	/// distributed out of process caching.
	/// </summary>
	public interface ILongTermCache
	{
		/// <summary>
		/// Will get a strongly typed object from longterm cache.  Will return null if the item does not exist.
		/// </summary>
		/// <typeparam name="T">The type of object to retreive.</typeparam>
		/// <param name="cacheKey">The cache key of the cached item.</param>
		/// <returns></returns>
		T Get<T>(string cacheKey) where T : class;
	}
}
