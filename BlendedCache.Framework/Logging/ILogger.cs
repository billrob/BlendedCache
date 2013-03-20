using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Logging
{
	/// <summary>
	/// Represents an instance of a Logger.
	/// </summary>
	public interface ILogger
	{
		void Debug(string message);
		void Debug(string message, Exception exception);
		void DebugFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args);
		void DebugFormat(Exception exception, string format, params object[] args);
		void DebugFormat(IFormatProvider formatProvider, string format, params object[] args);
		void DebugFormat(string format, params object[] args);
		void Error(string message);
		void Error(string message, Exception exception);
		void ErrorFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args);
		void ErrorFormat(Exception exception, string format, params object[] args);
		void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args);
		void ErrorFormat(string format, params object[] args);
		void Fatal(string message);
		void Fatal(string message, Exception exception);
		void FatalFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args);
		void FatalFormat(Exception exception, string format, params object[] args);
		void FatalFormat(IFormatProvider formatProvider, string format, params object[] args);
		void FatalFormat(string format, params object[] args);
		void Info(string message);
		void Info(string message, Exception exception);
		void InfoFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args);
		void InfoFormat(Exception exception, string format, params object[] args);
		void InfoFormat(IFormatProvider formatProvider, string format, params object[] args);
		void InfoFormat(string format, params object[] args);
		bool IsDebugEnabled { get; }
		bool IsErrorEnabled { get; }
		bool IsFatalEnabled { get; }
		bool IsInfoEnabled { get; }
		bool IsWarnEnabled { get; }
		void Warn(string message);
		void Warn(string message, Exception exception);
		void WarnFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args);
		void WarnFormat(Exception exception, string format, params object[] args);
		void WarnFormat(IFormatProvider formatProvider, string format, params object[] args);
		void WarnFormat(string format, params object[] args);
	}
}
