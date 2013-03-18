using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Interface for injecting the BlendedCache container..
	/// </summary>
	/// <typeparam name="TDataLoader">The type of the data loader for the cache interaction.</typeparam>
	public interface IBlendedCache<TDataLoader>
	{
		/// <summary>
		/// Will turn on/off flush mode for the cache if not already activated.  When turned on initially 
		/// the ContextCache is emptied but will cache further get calls to ensure data consistency through a 
		/// give code path.  LongTerm and Volatile will always return null when flush mode is activated.  Use sparingly
		/// </summary>
		/// <param name="flushMode">Whether flush mode should be turned on or off.</param>
		/// <returns></returns>
		bool SetFlushMode(bool flushMode);
	}
	
}