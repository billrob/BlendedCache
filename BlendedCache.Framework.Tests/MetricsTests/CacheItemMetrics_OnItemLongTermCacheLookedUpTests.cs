using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMM = Rhino.Mocks.MockRepository;
using RME = Rhino.Mocks.RhinoMocksExtensions;

namespace BlendedCache.Tests.MetricsTests
{
	[TestFixture]
	public class CacheItemMetrics_OnItemLongTermCacheLookedUpTests
	{
		private CacheItemMetrics _metrics;
		private object _itemLookedUp;
		private IWebRequestCacheMetricsUpdater _updaterMock;

		[SetUp]
		public void SetUp()
		{
			_itemLookedUp = null;
			_metrics = new CacheItemMetrics("");

			_updaterMock = RMM.GenerateStrictMock<IWebRequestCacheMetricsUpdater>();
			RME.Stub(_updaterMock, x => x.Increment_Cache_LongTermLookup());
			RME.Stub(_updaterMock, x => x.Increment_Cache_LongTermHits());
			RME.Stub(_updaterMock, x => x.Increment_Cache_LongTermMisses());
		}

		[Test]
		public void when_executing_should_increase_LongTermLookUpCount()
		{
			var before = _metrics.GetValue("_longTermCacheLookUps");

			Execute();

			Assert.Less(before, _metrics.GetValue("_longTermCacheLookUps"));
		}

		[Test]
		public void when_executing_should_call_Increment_Cache_LongTermLookup()
		{
			Execute();

			RME.AssertWasCalled(_updaterMock, x=>x.Increment_Cache_LongTermLookup(), opt => opt.Repeat.Once());
		}

		[Test]
		public void when_executing_item_not_found_should_call_Increment_Cache_LongTermMisses()
		{
			_itemLookedUp = null;

			Execute();

			RME.AssertWasCalled(_updaterMock, x=>x.Increment_Cache_LongTermMisses(), opt => opt.Repeat.Once());
		}

		[Test]
		public void when_executing_item_not_found_should_not_call_Increment_Cache_LongTermHits()
		{
			_itemLookedUp = null;

			Execute();

			RME.AssertWasNotCalled(_updaterMock, x=>x.Increment_Cache_LongTermHits());
		}

		[Test]
		public void when_executing_item_not_found_should_increment_LongTermCacheMisses()
		{
			_itemLookedUp = null;
			var before = _metrics.GetValue("_longTermCacheMisses");

			Execute();

			Assert.Less(before, _metrics.GetValue("_longTermCacheMisses"));
		}

		[Test]
		public void when_executing_item_found_should_call_Increment_Cache_LongTermHits()
		{
			_itemLookedUp = new object();

			Execute();

			RME.AssertWasCalled(_updaterMock, x => x.Increment_Cache_LongTermHits(), opt => opt.Repeat.Once());
		}

		[Test]
		public void when_executing_item_found_should_not_call_Increment_Cache_LongTermMisses()
		{
			_itemLookedUp = new object();

			Execute();

			RME.AssertWasNotCalled(_updaterMock, x => x.Increment_Cache_LongTermMisses());
		}

		[Test]
		public void when_executing_item_found_should_increment_LongTermCacheHits()
		{
			_itemLookedUp = new object();
			var before = _metrics.GetValue("_longTermCacheHits");

			Execute();

			Assert.Less(before, _metrics.GetValue("_longTermCacheHits"));
		}
		private void Execute()
		{
			_metrics.OnItemLongTermCacheLookedUp(_itemLookedUp, _updaterMock);
		}
	}
}
