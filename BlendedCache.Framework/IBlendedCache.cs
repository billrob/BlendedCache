using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Interface for injecting the BlendedCache container.
	/// </summary>
	public interface IBlendedCache
	{
		/// <summary>
		/// Will turn on/off flush mode for the cache if not already activated.  When turned on initially 
		/// the ContextCache is emptied but will cache further get calls to ensure data consistency through a 
		/// give code path.  LongTerm and Volatile will always return null when flush mode is activated.  Use sparingly
		/// 
		/// Useful for forcing a reload of an item through all cache layers (e.g., after changes to an object are save to the db)
		/// </summary>
		/// <param name="flushMode">Whether flush mode should be turned on or off.</param>
		/// <returns></returns>
		bool SetFlushMode(bool flushMode);

		/// <summary>
		/// Will get the specified item based on the cacheKey.
		/// </summary>
		/// <typeparam name="TData">The type of data that should be returned.</typeparam>
		/// <param name="lookupKey">The lookupKey, eg. cacheKey or primaryKey of the item to be retrieved.</param>
		/// <returns>The item requests or null.  If TypeConfigurations are registered, the DataLoader will be executed.</returns>
		TData Get<TData, TKey>(TKey lookupKey) where TData : class;
		TData Get<TData>(int lookupKey) where TData : class;
		TData Get<TData>(string lookupKey) where TData : class;

		/// <summary>
		/// Will set the cache item using the TypeConfiguration or the default settings.
		/// </summary>
		/// <typeparam name="TData">The type of data that should be set.</typeparam>
		/// <typeparam name="TKey">The type of the lookupKey to retreive this item.</typeparam>
		/// <param name="lookupKey">The cacheKey of the item to be stored.</param>
		/// <param name="cachedItem">The data that should be stored under the lookupKey.</param>
		void Set<TData, TKey>(TKey lookupKey, TData cachedItem) where TData : class;
	}
}