using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BlendedCache.Logging.Log4Net
{
	/// <summary>
	/// Represents an instance of BlendedCache.Logging.ILoggerFactory provided by a log4net version.
	/// </summary>
	public class Log4NetLoggerFactory : BlendedCache.Logging.ILoggerFactory
	{
		static Log4NetLoggerFactory()
		{
			// TODO: logging configuration handling/overrides
		}

		static log4net.ILog GetLog4NetInstance(String name)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			// we're going to return a new one per call
			return log4net.LogManager.GetLogger(name);
		}
		static log4net.ILog GetLog4NetInstance(Type declaringType)
		{
			if (declaringType == null)
				throw new ArgumentNullException("declaringType");

			// we're going to return a new one per call
			return log4net.LogManager.GetLogger(declaringType);
		}

		BlendedCache.Logging.ILogger ILoggerFactory.GetLogger()
		{
			var logger = Log4NetLoggerFactory.GetLog4NetInstance(MethodBase.GetCurrentMethod().DeclaringType);
			return new Log4NetLogger(logger);
		}

		BlendedCache.Logging.ILogger ILoggerFactory.GetLogger(string loggerName)
		{
			if (String.IsNullOrEmpty(loggerName))
				throw new ArgumentOutOfRangeException("loggerName");

			var logger = Log4NetLoggerFactory.GetLog4NetInstance(loggerName);
			return new Log4NetLogger(logger);
		}
	}
}
