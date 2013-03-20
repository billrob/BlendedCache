using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Provides a null context cache experience always returning null, and never throwing an exception.
	/// </summary>
	public class NullContextCache : IContextCache
	{
		/// <summary>
		/// Will do nothing.
		/// </summary>
		void IContextCache.Set<T>(string cacheKey, T value)
		{
		}

		/// <summary>
		/// Will always return default(T) which should be null.
		/// </summary>
		T IContextCache.Get<T>(string cacheKey)
		{
			return default(T);
		}

		/// <summary>
		/// Will return an empty collection.
		/// </summary>
		IEnumerable<string> IContextCache.Keys
		{
			get { return Enumerable.Empty<string>(); }
		}

		/// <summary>
		/// Will do nothing.
		/// </summary>
		void IContextCache.Remove(string cacheKey)
		{

		}
	}
}
