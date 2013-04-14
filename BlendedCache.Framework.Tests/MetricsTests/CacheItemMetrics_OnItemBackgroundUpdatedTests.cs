using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Tests.MetricsTests
{
	[TestFixture]
	public class CacheItemMetrics_OnItemBackgroundUpdated
	{
		private CachedItemMetrics _metrics;
		private long _ticksStart;
		private long _ticksEnd;

		[SetUp]
		public void SetUp()
		{
			_ticksStart = 38724234;
			_ticksEnd = _ticksStart + 10000;
			_metrics = new CachedItemMetrics("", new object());
		}

		[Test]
		public void when_executing_should_increment_TimesBackgroundLoaded()
		{
			var before = _metrics.GetValue("_timesBackgroundLoaded");

			Execute();

			Assert.AreEqual(before + 1, _metrics.GetValue("_timesBackgroundLoaded"));
		}

		[Test]
		public void when_executing_should_not_increment_TimesBackgroundLoadFailed()
		{
			var before = _metrics.GetValue("_timesBackgroundLoadFailed");

			Execute();

			Assert.AreEqual(before, _metrics.GetValue("_timesBackgroundLoadFailed"));
		}

		[Test]
		public void when_executing_should_increment_TimeInBackgroundLoad()
		{
			var before = _metrics.GetValue("_timeInBackgroundLoad");

			Execute();

			Assert.Less(before, _metrics.GetValue("_timeInBackgroundLoad"));
			Assert.AreNotEqual(0, _metrics.GetValue("_timeInBackgroundLoad"));
			Assert.AreEqual(_ticksEnd - _ticksStart, _metrics.GetValue("_timeInBackgroundLoad"));
		}

		[Test]
		public void when_executing_should_set_LastLoaded()
		{
			var before = _metrics.GetValue("_lastLoaded");

			Execute();

			Assert.AreNotEqual(before, _metrics.GetValue("_lastLoaded"));
			Assert.AreEqual(_ticksEnd, _metrics.GetValue("_lastLoaded"));
			Assert.AreNotEqual(0, _metrics.GetValue("_lastLoaded"));
		}

		[Test]
		public void when_executing_should_overflow_increment_TimeInBackgroundLoad()
		{
			_metrics.SetValue("_timeInBackgroundLoad", Int64.MaxValue - 1);

			Execute();

			Assert.AreEqual(Int64.MaxValue, _metrics.GetValue("_timeInBackgroundLoad"));
			Assert.AreNotEqual(0, _metrics.GetValue("_timeInBackgroundLoad"));
		}

		[Test]
		public void when_executing_with_ticksStart_of_zero_should_increment_TimesBackgroundLoaded()
		{
			_ticksStart = 0;
			var before = _metrics.GetValue("_timesBackgroundLoaded");

			Execute();

			Assert.AreEqual(before + 1, _metrics.GetValue("_timesBackgroundLoaded"));
		}

		[Test]
		public void when_executing_with_ticksStart_of_zero_should_increment_TimesBackgroundLoadFailed()
		{
			_ticksStart = 0;
			var before = _metrics.GetValue("_timesBackgroundLoadFailed");

			Execute();

			Assert.AreEqual(before + 1, _metrics.GetValue("_timesBackgroundLoadFailed"));
		}

		[Test]
		public void when_executing_with_ticksStart_of_zero_should_not_increment_TimeInBackgroundLoad()
		{
			_ticksStart = 0;
			var before = _metrics.GetValue("_timeInBackgroundLoad");

			Execute();

			Assert.AreEqual(before, _metrics.GetValue("_timeInBackgroundLoad"));
		}

		public void Execute()
		{
			_metrics.OnItemBackgroundUpdated(_ticksStart, _ticksEnd);
		}
	}
}
