using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Tests
{
	static class DateTimeExtensions
	{
		private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// Will convert the date time object into a unix timestamp.
		/// </summary>
		/// <param name="datetime"></param>
		/// <returns></returns>
		public static long ToUnixTimestamp(this DateTime datetime)
		{
			var timeSpan = datetime.ToUniversalTime() - _epoch;

			return (long)Math.Floor(timeSpan.TotalSeconds);
		}

		/// <summary>
		/// Will take a unix timestamp and convert to a date time object.
		/// </summary>
		/// <param name="timestamp"></param>
		/// <returns></returns>
		public static DateTime FromUnixTimestamp(Int64 timestamp)
		{
			return _epoch.AddSeconds(timestamp);
		}
	}
}
