using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Represents a level of logging.
	/// </summary>
	[Flags]
	public enum LogLevel
	{
		/// <summary>
		/// Indicates a level of Trace logging.
		/// </summary>
		Trace = 1,
		/// <summary>
		/// Indicates a level of Debug logging.
		/// </summary>
		Debug = 2,
		/// <summary>
		/// Indicates a level of Info logging.
		/// </summary>
		Info = 4,
		/// <summary>
		/// Indicates a level of Warn logging.
		/// </summary>
		Warn = 8,
		/// <summary>
		/// Indicates a level of Error logging.
		/// </summary>
		Error = 16,
		/// <summary>
		/// Indicates a level of Fatal logging.
		/// </summary>
		Fatal = 32,
		/// <summary>
		/// Indicates no logging.
		/// </summary>
		None = 64,
	}
}
