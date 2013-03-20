using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Logging
{
	internal class LoggerConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty("minimalLoggingLevel")]
		public LogLevel MinimalLoggingLevel
		{
			get 
			{
				var logLevelValue = (String)this["minimalLoggingLevel"];
				LogLevel logLevel = LogLevel.None;
				if (false == Enum.TryParse(logLevelValue, out logLevel))
				{
					return LogLevel.None;
				}
				return logLevel;
			}
		}

		public Type DefaultLoggerFactoryType
		{
			get
			{
				throw new NotImplementedException();
			}
		}

	}
}
