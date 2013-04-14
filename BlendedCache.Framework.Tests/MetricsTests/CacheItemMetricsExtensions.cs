using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Tests.MetricsTests
{
	internal static class CacheItemMetricsExtensions
	{
		/// <summary>
		/// Will set the value on the private field of the metrics object.
		/// </summary>
		internal static void SetValue(this CachedItemMetrics metrics, string fieldName, long value)
		{
			var fieldInfo = metrics.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
			if (fieldInfo == null)
				throw new ApplicationException("Could not find field name: " + fieldName + " on CacheItemMetrics object.");

			fieldInfo.SetValue(metrics, value);
		}

		/// <summary>
		/// Will get the value from the private field of the metrics object.
		/// </summary>
		internal static long GetValue(this CachedItemMetrics metrics, string fieldName)
		{
			var fieldInfo = metrics.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
			if (fieldInfo == null)
				throw new ApplicationException("Could not find field name: " + fieldName + " on CacheItemMetrics object.");

			return (long)fieldInfo.GetValue(metrics);
		}

	}
}