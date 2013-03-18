using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	//todo:0 figure out how to plug in an agnostic logger
	public static class Logger
	{
		/// <summary>
		/// Will log the message to the log provider as an info.
		/// </summary>
		/// <param name="message">The message for the log entry.</param>
		public static void Info(string message)
		{
			//somehow wraps the logger write command.
		}
	}
}
