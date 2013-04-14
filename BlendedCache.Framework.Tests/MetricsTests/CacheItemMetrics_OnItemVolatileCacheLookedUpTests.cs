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
	public class CacheItemMetrics_OnItemVolatileCacheLookedUpTests
	{
		private CachedItemMetrics _metrics;
		private object _itemLookedUp;
		private IWebRequestCacheMetricsUpdater _updaterMock;

		[SetUp]
		public void SetUp()
		{
			_itemLookedUp = null;
			_metrics = new CachedItemMetrics("", new object());

			_updaterMock = RMM.GenerateStrictMock<IWebRequestCacheMetricsUpdater>();
			RME.Stub(_updaterMock, x => x.Increment_Cache_VolatileLookup());
			RME.Stub(_updaterMock, x => x.Increment_Cache_VolatileHits());
			RME.Stub(_updaterMock, x => x.Increment_Cache_VolatileMisses());
		}

		[Test]
		public void when_executing_should_increase_VolatileLookUpCount()
		{
			var before = _metrics.GetValue("_volatileCacheLookUps");

			Execute();

			Assert.Less(before, _metrics.GetValue("_volatileCacheLookUps"));
		}

		[Test]
		public void when_executing_should_call_Increment_Cache_VolatileLookup()
		{
			Execute();

			RME.AssertWasCalled(_updaterMock, x=>x.Increment_Cache_VolatileLookup(), opt => opt.Repeat.Once());
		}

		[Test]
		public void when_executing_item_not_found_should_call_Increment_Cache_VolatileMisses()
		{
			_itemLookedUp = null;

			Execute();

			RME.AssertWasCalled(_updaterMock, x=>x.Increment_Cache_VolatileMisses(), opt => opt.Repeat.Once());
		}

		[Test]
		public void when_executing_item_not_found_should_not_call_Increment_Cache_VolatileHits()
		{
			_itemLookedUp = null;

			Execute();

			RME.AssertWasNotCalled(_updaterMock, x=>x.Increment_Cache_VolatileHits());
		}

		[Test]
		public void when_executing_item_not_found_should_increment_VolatileCacheMisses()
		{
			_itemLookedUp = null;
			var before = _metrics.GetValue("_volatileCacheMisses");

			Execute();

			Assert.Less(before, _metrics.GetValue("_volatileCacheMisses"));
		}

		[Test]
		public void when_executing_item_found_should_call_Increment_Cache_VolatileHits()
		{
			_itemLookedUp = new object();

			Execute();

			RME.AssertWasCalled(_updaterMock, x => x.Increment_Cache_VolatileHits(), opt => opt.Repeat.Once());
		}

		[Test]
		public void when_executing_item_found_should_not_call_Increment_Cache_VolatileMisses()
		{
			_itemLookedUp = new object();

			Execute();

			RME.AssertWasNotCalled(_updaterMock, x => x.Increment_Cache_VolatileMisses());
		}

		[Test]
		public void when_executing_item_found_should_increment_VolatileCacheHits()
		{
			_itemLookedUp = new object();
			var before = _metrics.GetValue("_volatileCacheHits");

			Execute();

			Assert.Less(before, _metrics.GetValue("_volatileCacheHits"));
		}
		private void Execute()
		{
			_metrics.OnItemVolatileCacheLookedUp(_itemLookedUp, _updaterMock);
		}
	}
}
