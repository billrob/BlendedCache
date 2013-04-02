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
	public class Log4NetLoggerFactory : LoggerFactoryBase
	{
		/// <summary>
		/// Instantiates an instance of a Log4NetLoggerFactory.
		/// </summary>
		/// <param name="config"></param>
		public Log4NetLoggerFactory()
		{
		}

		#region required abstract members

		/// <summary>
		/// Not recommended in log4net as it will return a named logger using this class name, Log4NetLoggerFactory.
		/// </summary>
		/// <returns></returns>
		public override ILogger GetLogger()
		{
			return this.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.FullName);
		}

		public override ILogger GetLogger(string loggerName)
		{
			if (String.IsNullOrEmpty(loggerName))
				throw new ArgumentOutOfRangeException("loggerName");

			var logger = log4net.LogManager.GetLogger(loggerName);
			return new Log4NetLogger(logger);
		}

		#endregion
	}
}
