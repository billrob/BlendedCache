using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Logging
{
	/// <summary>
	/// Represents base logger support.
	/// </summary>
	internal static class LoggerHelper
	{
		// If moving to IoC, we would get rid of this "Helper" class and register a LoggerFactory instead.
		//

		// keep the static instance around so we can change runtime variables.
		private static Logging.ILoggerFactory LoggerFactory;

		/// <summary>
		/// Gets an instance of a logger with the specified declaring type name.
		/// </summary>
		/// <param name="loggerName"></param>
		internal static ILogger GetLogger(Type type)
		{
			// previously supported: System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName

			if (type == null)
				throw new ArgumentNullException("type");

			// this eventually names the logger we are using so it shows up
			var loggerName = type.FullName;

			if (LoggerHelper.LoggerFactory == null)
			{
				// instantiate the configured log factory
				//
				Logging.ILoggerFactory factory = null;
				var section = System.Configuration.ConfigurationManager.GetSection("BlendedCache/Logging") as Logging.LoggerConfigurationSection;
				if ((section != null) && (section.LoggerFactoryType != null))
				{
					factory = Activator.CreateInstance(section.LoggerFactoryType) as Logging.ILoggerFactory;
					if (factory == null)
						throw new ConfigurationErrorsException(String.Format("Could not create type type: {0}", section.LoggerFactoryType));
				}

				// the reason for grabbing the factory is so we can optionally expose an SetLoggerLevel() on this class.
				// was thinking of creating a method on this BlendedCache object to change the logging level.  admit, 
				// far too rich right now but can be added later.
				//
				LoggerHelper.LoggerFactory = factory ?? new Logging.NullLoggerFactory();
			}

			// get it!
			return LoggerHelper.LoggerFactory.GetLogger(loggerName);
		}
	}
}
