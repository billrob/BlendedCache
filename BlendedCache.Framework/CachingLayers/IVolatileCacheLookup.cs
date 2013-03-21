﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Internal class to make testing easier and to break up the actual lookup logic for context cache.
	/// </summary>
	internal interface IVolatileCacheLookup
	{
		/// <summary>
		/// Will try to get the specified cacheKey from cache as the provided TData type.
		/// </summary>
		/// <typeparam name="TData"></typeparam>
		/// <param name="fixedUpCacheKey">The cacheKey after it has been run through fix up.</param>
		/// <returns>The existing item if it exists.</returns>
		TData GetDataFromVolatileCache<TData>(string fixedUpCacheKey) where TData : class;
	}
}
