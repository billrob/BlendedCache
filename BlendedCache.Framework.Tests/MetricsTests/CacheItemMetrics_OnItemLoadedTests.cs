using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Tests.MetricsTests
{
	[TestFixture]
	public class CacheItemMetrics_OnItemLoadedTests
	{
		private CachedItemMetrics _metrics;
		private long _ticksStart;
		private long _ticksEnd;
		private bool _flushed;

		[SetUp]
		public void SetUp()
		{
			_flushed = false;
			_ticksStart = 3234234;
			_ticksEnd = _ticksStart + 10000;
			_metrics = new CachedItemMetrics("", new object());
		}

		[Test]
		public void should_set_FirstLoaded_only_when_FirstLoaded_is_zero()
		{
			_metrics.SetValue("_firstLoaded", 0);

			Execute();

			Assert.AreNotEqual(0, _metrics.GetValue("_firstLoaded"));
			Assert.AreEqual(_ticksEnd, _metrics.GetValue("_firstLoaded"));
		}

		[Test]
		public void should_not_set_FirstLoaded_when_FirstLoaded_is_not_zero()
		{
			var before =3242342;
			_metrics.SetValue("_firstLoaded", before);

			Execute();

			Assert.AreNotEqual(0, _metrics.GetValue("_firstLoaded"));
			Assert.AreEqual(before, _metrics.GetValue("_firstLoaded"));
		}

		[Test]
		public void should_set_LastLoaded_time()
		{
			var before = _metrics.GetValue("_firstLoaded");

			Execute();

			Assert.AreNotEqual(before, _metrics.GetValue("_firstLoaded"));
			Assert.AreEqual(_ticksEnd, _metrics.GetValue("_firstLoaded"));
		}

		[Test]
		public void should_increment_TimesLoaded()
		{
			var before = _metrics.GetValue("_timesLoaded");

			Execute();

			Assert.AreEqual(before + 1, _metrics.GetValue("_timesLoaded"));
		}

		[Test]
		public void should_not_increment_TimesFlushed()
		{
			var before = _metrics.GetValue("_timesFlushed");

			Execute();

			Assert.AreEqual(before, _metrics.GetValue("_timesFlushed"));
		}

		[Test]
		public void when_flush_mode_should_increment_TimesFlushed()
		{
			_flushed = true;
			var before = _metrics.GetValue("_timesFlushed");

			Execute();

			Assert.AreEqual(before + 1, _metrics.GetValue("_timesFlushed"));
		}

		[Test]
		public void when_executing_should_increment_TimeInLoad()
		{
			var before = _metrics.GetValue("_timeInLoad");

			Execute();

			Assert.Less(before, _metrics.GetValue("_timeInLoad"));
			Assert.AreNotEqual(0, _metrics.GetValue("_timeInLoad"));
			Assert.AreEqual(_ticksEnd - _ticksStart, _metrics.GetValue("_timeInLoad"));
		}

		[Test]
		public void when_executing_should_overflow_increment_TimeInLoad()
		{
			_metrics.SetValue("_timeInLoad", Int64.MaxValue - 1);

			Execute();

			Assert.AreEqual(Int64.MaxValue, _metrics.GetValue("_timeInLoad"));
			Assert.AreNotEqual(0, _metrics.GetValue("_timeInLoad"));
		}



		public void Execute()
		{
			_metrics.OnItemLoaded(_ticksStart, _ticksEnd, _flushed);
		}
	}
}
