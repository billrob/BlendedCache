using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlendedCache.Logging.Log4Net
{
	/// <summary>
	/// Represents an instance of a log4net version of BlendedCache.Logging.ILogger.
	/// </summary>
	public class Log4NetLogger : BlendedCache.Logging.ILogger
	{
		private log4net.ILog _logger;

		/// <summary>
		/// Creates a instance of a log4net version of BlendedCache.Logging.ILogger.
		/// </summary>
		/// <param name="logger"></param>
		public Log4NetLogger(log4net.ILog logger)
		{
			this._logger = logger;
		}

		#region BlendedCache.Logging.ILogger implementation

		void ILogger.Debug(string message)
		{
			_logger.Debug(message);
		}

		void ILogger.Debug(string message, Exception exception)
		{
			_logger.Debug(message, exception);
		}

		void ILogger.DebugFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
		{
			_logger.DebugFormat(formatProvider, FormatException(exception, format), args);
		}

		void ILogger.DebugFormat(Exception exception, string format, params object[] args)
		{
			_logger.DebugFormat(FormatException(exception, format), args);
		}

		void ILogger.DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
			_logger.DebugFormat(formatProvider, format, args);
		}

		void ILogger.DebugFormat(string format, params object[] args)
		{
			_logger.DebugFormat(format, args);
		}

		void ILogger.Error(string message)
		{
			_logger.Error(message);
		}

		void ILogger.Error(string message, Exception exception)
		{
			_logger.Error(message, exception);
		}

		void ILogger.ErrorFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
		{
			_logger.ErrorFormat(formatProvider, FormatException(exception, format), args);
		}

		void ILogger.ErrorFormat(Exception exception, string format, params object[] args)
		{
			_logger.ErrorFormat(FormatException(exception, format), args);
		}

		void ILogger.ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
			_logger.ErrorFormat(formatProvider, format, args);
		}

		void ILogger.ErrorFormat(string format, params object[] args)
		{
			_logger.ErrorFormat(format, args);
		}

		void ILogger.Fatal(string message)
		{
			_logger.Fatal(message);
		}

		void ILogger.Fatal(string message, Exception exception)
		{
			_logger.Fatal(message, exception);
		}

		void ILogger.FatalFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
		{
			_logger.FatalFormat(formatProvider, FormatException(exception, format), args);
		}

		void ILogger.FatalFormat(Exception exception, string format, params object[] args)
		{
			_logger.FatalFormat(FormatException(exception, format), args);
		}

		void ILogger.FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
			_logger.FatalFormat(formatProvider, format, args);
		}

		void ILogger.FatalFormat(string format, params object[] args)
		{
			_logger.FatalFormat(format, args);
		}

		void ILogger.Info(string message)
		{
			_logger.Info(message);
		}

		void ILogger.Info(string message, Exception exception)
		{
			_logger.Info(message, exception);
		}

		void ILogger.InfoFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
		{
			_logger.InfoFormat(formatProvider, FormatException(exception, format), args);
		}

		void ILogger.InfoFormat(Exception exception, string format, params object[] args)
		{
			_logger.InfoFormat(FormatException(exception, format), args);
		}

		void ILogger.InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
			_logger.InfoFormat(formatProvider, format, args);
		}

		void ILogger.InfoFormat(string format, params object[] args)
		{
			_logger.InfoFormat(format, args);
		}

		bool ILogger.IsDebugEnabled
		{
			get { return _logger.IsDebugEnabled; }
		}

		bool ILogger.IsErrorEnabled
		{
			get { return _logger.IsErrorEnabled; }
		}

		bool ILogger.IsFatalEnabled
		{
			get { return _logger.IsFatalEnabled; }
		}

		bool ILogger.IsInfoEnabled
		{
			get { return _logger.IsInfoEnabled; }
		}

		bool ILogger.IsWarnEnabled
		{
			get { return _logger.IsWarnEnabled; }
		}

		void ILogger.Warn(string message)
		{
			_logger.Warn(message);
		}

		void ILogger.Warn(string message, Exception exception)
		{
			_logger.Warn(message, exception);
		}

		void ILogger.WarnFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
		{
			_logger.WarnFormat(formatProvider, FormatException(exception, format), args);
		}

		void ILogger.WarnFormat(Exception exception, string format, params object[] args)
		{
			_logger.WarnFormat(FormatException(exception, format), args);
		}

		void ILogger.WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
		{
			_logger.WarnFormat(formatProvider, format, args);
		}

		void ILogger.WarnFormat(string format, params object[] args)
		{
			_logger.WarnFormat(format, args);
		} 
		#endregion

		#region static private helpers

		private static String FormatException(Exception exception, String format)
		{
			// log4net doesn't support exception handling for DebugFormat() or related handling like NLog.  so,
			// in this method we combine it with the format string that is normally used in the logger message by 
			// appending it to the end.
			//
			return String.Format("{0} \n{1}", format, exception.ToString());
		}

		#endregion
	}
}
