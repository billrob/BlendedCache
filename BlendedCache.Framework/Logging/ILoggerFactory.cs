using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Logging
{
	/// <summary>
	/// Represents an instance of ILogFactory.
	/// </summary>
	public interface ILoggerFactory
	{
		/// <summary>
		/// Returns an instance of the configured Logger.
		/// </summary>
		/// <returns></returns>
		ILogger GetLogger();
		/// <summary>
		/// Returns a named instance of the configured Logger.
		/// </summary>
		/// <param name="loggerName"></param>
		/// <returns></returns>
		ILogger GetLogger(string loggerName);
		/// <summary>
		/// Changes the minimal level of logging for the application.
		/// </summary>
		/// <param name="logLevel">The minimal level to log to the underlying log provider.</param>
		/// <remarks>Use this method to change the AppDomain's level for all logging.  
		/// The order is from lowest to highest, which is defined as: Trace -> Debug -> Info -> Warn -> Error -> Fatal.
		/// An example would be set it to LogLevel.Info.  This would log ann Info, Warn, Error and Fatals, but ignore Trace and Debug levels.</remarks>
		void SetLoggerLevel(BlendedCache.Logging.LogLevel logLevel);
	}
}
