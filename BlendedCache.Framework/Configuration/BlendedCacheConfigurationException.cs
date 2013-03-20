using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Contains information about an exception that occured during the configuration.  These exceptions are 
	/// fixable by a developer because something is wrong with setup.
	/// </summary>
	public class BlendedCacheConfigurationException : ApplicationException
	{
		public BlendedCacheConfigurationException(string message, Exception ex)
			: base(message, ex) { }
		public BlendedCacheConfigurationException(string message)
			: base(message) { }
	}
}
