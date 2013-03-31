using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// The volatile cache interface.  Typically this will be in memory caching in the process space.
	/// </summary>
	public interface IVolatileCache
	{
		/// <summary>
		/// Will set the given item in cache with for the cache durations specified.
		/// </summary>
		/// <typeparam name="TData">The type of the object. Normally infered from datatype.</typeparam>
		/// <param name="cacheKey">The cacheKey for the item.</param>
		/// <param name="cachedItem">The actual item to be stored in volatile cache.</param>
		/// <param name="cacheDurationSeconds">The duration in seconds this item should be stored.</param>
		void Set<TData>(string cacheKey, TData cachedItem, int cacheDurationSeconds) where TData : class;

		/// <summary>
		/// Will get a strongly typed object from volatile cache.  Will return null if the item does not exist.
		/// </summary>
		/// <typeparam name="TData">The type of object to retreive.</typeparam>
		/// <param name="cacheKey">The cache key of the cached item.</param>
		/// <returns>Will return the item from cache or null if it doesn't exist.</returns>
		TData Get<TData>(string cacheKey) where TData : class;

		/// <summary>
		/// Will remove the specified item from the context cache.  No action is taken if the cacheKey does not exist.
		/// </summary>
		/// <param name="cacheKey">The cacheKey to be removed.</param>
		void Remove(string cacheKey);
	}
}
