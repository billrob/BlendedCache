using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Tests
{
	/// <summary>
	/// mocked object to make it easier to work with cache timeouts.
	/// </summary>
	internal class CacheTimeoutMock : ICacheTimeout
	{
		public int VolatileTimeoutInSeconds { get; set; }
		public int LongTermTimeoutInSeconds { get; set; }
		public int LongTermRefreshInSeconds { get; set; }
	}
}
