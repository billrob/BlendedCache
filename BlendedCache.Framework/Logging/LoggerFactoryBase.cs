using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Logging
{
	/// <summary>
	/// Represents a base logger factory that can be implemented in a custom class.
	/// </summary>
	public abstract class LoggerFactoryBase : ILoggerFactory
	{
		private LoggerConfigurationSection _configurationSection;

		/// <summary>
		/// Instantiates the base factory using the build-in configurationSection.
		/// </summary>
		public LoggerFactoryBase()
			: this(ConfigurationManager.GetSection("BlendedCache/Logging") as ConfigurationSection)
		{
		}
		/// <summary>
		/// Instantiates the base factory using the build-in configurationSection.
		/// </summary>
		/// <param name="configurationSection">An application configured section that casts to a LoggerConfigurationSection.</param>
		public LoggerFactoryBase(ConfigurationSection configurationSection)
		{
			if (configurationSection == null)
				throw new ArgumentNullException("configurationSection");
			
			// cast it to what we expect
			var section = configurationSection as LoggerConfigurationSection;
			if (section == null)
				throw new ConfigurationErrorsException("Required BlendedCache/Logging section not found in the application configuration file.");

			_configurationSection = section;
		}

		/// <summary>
		/// Gets the current level of logging set for the application.
		/// </summary>
		internal LogLevel LogLevel
		{
			get
			{
				return _configurationSection.LoggerLevelMinimal;
			}
		}

		/// <summary>
		/// Changes the minimal level of logging for the application.
		/// </summary>
		/// <param name="logLevel">The minimal level to log to the underlying log provider.</param>
		/// <remarks>Use this method to change the AppDomain's level for all logging.  
		/// The order is from lowest to highest, which is defined as: Trace -> Debug -> Info -> Warn -> Error -> Fatal.
		/// An example would be set it to LogLevel.Info.  This would log ann Info, Warn, Error and Fatals, but ignore Trace and Debug levels.</remarks>
		public virtual void SetLoggerLevel(LogLevel logLevel)
		{
			_configurationSection.LoggerLevelMinimal = logLevel;
		}

		/// <summary>
		/// Returns an instance of the configured Logger.
		/// </summary>
		/// <returns></returns>
		public abstract ILogger GetLogger();
		/// <summary>
		/// Returns a named instance of the configured Logger.
		/// </summary>
		/// <param name="loggerName"></param>
		/// <returns></returns>
		public abstract ILogger GetLogger(string loggerName);
	}
}
