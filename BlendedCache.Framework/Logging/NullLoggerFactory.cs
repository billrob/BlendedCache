using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Logging
{
	/// <summary>
	/// Represents an empty factory instance, usually for unit testing.
	/// </summary>
	internal class NullLoggerFactory : ILoggerFactory
	{
		private static ILogger Logger;

		static NullLoggerFactory()
		{
			NullLoggerFactory.Logger = new NullLogger();
		}

		ILogger ILoggerFactory.GetLogger()
		{
			return NullLoggerFactory.Logger;
		}

		ILogger ILoggerFactory.GetLogger(string loggerName)
		{
			return NullLoggerFactory.Logger;
		}
	}
}
