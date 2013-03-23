using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Interface for working with the context or thread based caching.
	/// </summary>
	public interface IContextCache
	{
		/// <summary>
		/// Will set the value via the cache key into the context cache.
		/// </summary>
		/// <typeparam name="T">The type of the object. Normally infered from datatype.</typeparam>
		/// <param name="key">The cache key for the context cache.</param>
		/// <param name="value">The value of the object to be cached.</param>
		void Set<T>(string key, T value) where T : class;
		
		/// <summary>
		/// Will get a strongly typed object from context cache.  Will return null if the item does not exist.
		/// </summary>
		/// <typeparam name="T">The type of object to retreive.</typeparam>
		/// <param name="cacheKey">The cache key of the cached item.</param>
		/// <returns></returns>
		T Get<T>(string cacheKey) where T : class;
		
		/// <summary>
		/// Will remove the specified item from the context cache.  No action is taken if the cacheKey does not exist.
		/// </summary>
		/// <param name="cacheKey">The cacheKey to be removed.</param>
		void Remove(string cacheKey);

		/// <summary>
		/// Will clear all of the items out of the caching container.
		/// </summary>
		void Clear();

	}
}