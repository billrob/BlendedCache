using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Logging
{
	/// <summary>
	/// Represents the application configuration section for Logging.
	/// </summary>
	public sealed class LoggerConfigurationSection : ConfigurationSection
	{
		private const string PROP_LOGGERLEVELMINIMAL = "loggerLevelMinimal";
		private const string PROP_LOGGERFACTORYTYPE = "loggerFactoryType";

		// Common exception messaging.
		private const string CONFIG_EXCEPTION = "Application configuration error under BlendedCache/Logging node: {0}, {1}";

		// The collection (property bag) that contains the section properties that can be modified at runtime. 
		private static ConfigurationPropertyCollection ConfigurationProperties;

		#region static configuration properties
		private static readonly ConfigurationProperty PropertyLoggerLevelMinimal =
			new ConfigurationProperty(PROP_LOGGERLEVELMINIMAL,
				typeof(BlendedCache.Logging.LogLevel),	// parses the type
				BlendedCache.Logging.LogLevel.None		// set the default
			);

		private static readonly ConfigurationProperty PropertyLoggerFactory =
			new ConfigurationProperty(PROP_LOGGERFACTORYTYPE,
				typeof(BlendedCache.Logging.ILoggerFactory)	// parses the type
			);
		#endregion

		/// <summary>
		/// Instantiates a new instance of LoggerConfigurationSection.
		/// </summary>
		public LoggerConfigurationSection()
		{
			// by using the instance ctor, instead of the static ctor, we can reset the static config
			// values at runtime in a separate file and re-read them with a new instance of this
			// class.
			//

			// read and setup the initial properties
			LoggerConfigurationSection.ConfigurationProperties = new ConfigurationPropertyCollection();
			LoggerConfigurationSection.ConfigurationProperties.Add(PropertyLoggerLevelMinimal);
			LoggerConfigurationSection.ConfigurationProperties.Add(PropertyLoggerFactory);
		}

		#region base overrides
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return LoggerConfigurationSection.ConfigurationProperties;
			}
		}
		#endregion

		/// <summary>
		/// Gets or sets the minimal logging level for the application.
		/// </summary>
		public LogLevel LoggerLevelMinimal
		{
			get 
			{
				return (BlendedCache.Logging.LogLevel)this[PROP_LOGGERLEVELMINIMAL];
			}
			set
			{
				this[PROP_LOGGERLEVELMINIMAL] = value;
			}
		}

		/// <summary>
		/// Gets or sets the factory type that implements BlendedCache.Logging.ILoggerFactory.
		/// </summary>
		public Type LoggerFactoryType
		{
			get
			{
				return (Type)this[PROP_LOGGERFACTORYTYPE];
			}
			set
			{
				this[PROP_LOGGERFACTORYTYPE] = value;
			}
		}
	}
}
