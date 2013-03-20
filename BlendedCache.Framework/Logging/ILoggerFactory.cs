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
	}
}
