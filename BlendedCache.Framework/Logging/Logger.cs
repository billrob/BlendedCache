using BlendedCache.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Represents common logging functions.
	/// </summary>
	internal static class Logger
	{
		private static LogLevel LogLevel;
		private static ILoggerFactory LoggerFactory;

		static Logger()
		{
			// set the default level of logging for the application.
			//
			LogLevel = BlendedCache.LogLevel.None;

			// instantiate the configured log factory
			//
			ILoggerFactory factory = null;
			var section = ConfigurationManager.GetSection("BlendedCache/Logging") as LoggerConfigurationSection;
			if ((section != null) && (section.DefaultLoggerFactoryType != null))
			{
				factory = Activator.CreateInstance(section.DefaultLoggerFactoryType) as ILoggerFactory;
			}
			Logger.LoggerFactory = factory ?? new NullLoggerFactory();
		}

		/// <summary>
		/// Changes the minimal level of logging for the application.
		/// </summary>
		/// <param name="logLevel">The minimal level to log to the underlying log provider.</param>
		/// <remarks>Use this method to change the AppDomain's level for all logging.  
		/// The order is from lowest to highest, which is defined as: Trace -> Debug -> Info -> Warn -> Error -> Fatal.
		/// An example would be set it to LogLevel.Info.  This would log ann Info, Warn, Error and Fatals, but ignore Trace and Debug levels.</remarks>
		public static void SetLoggerLevel(BlendedCache.LogLevel logLevel)
		{
			Logger.LogLevel = logLevel;
		}

		/// <summary>
		/// Logs a message to the configured ILoggerFactory.
		/// </summary>
		/// <param name="message">The message for the log entry.</param>
		public static void Info(string message)
		{
			LoggerFactory.GetLogger().Info(message);
		}
	}
}
