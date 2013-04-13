using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Framework.IntegrationTests.SimpleGetTests
{
	[TestFixture]
	public class FoundInContextCacheTests
	{
		private const string _lookupKey = "marco....pollo...chicken.eh";
		private string _cacheKey;
		private CachedData _contextCachedItem = null;
		private CachedData _response;
		private IContextCache _contextCache_Full;
		private Metrics _previousMetrics;
		private Metrics _metrics;
		
		[SetUp]
		public void SetUp()
		{
			_response = null;
			_cacheKey = new DefaultCacheKeyConverter().ConvertCacheKey<CachedData, string>("", _lookupKey);
			_contextCachedItem = new CachedData();
			_contextCache_Full = new DictionaryContextCache(_cacheKey, _contextCachedItem);

			_previousMetrics = BlendedCacheMetricsStore.GetCacheMetrics().SingleOrDefault(x => _cacheKey.Equals(x.CacheKey, StringComparison.OrdinalIgnoreCase)) ?? new Metrics();
		}

		[Test]
		public void when_in_ContextCache_should_return_ContextItem()
		{
			Execute();

			Assert.NotNull(_response);
			Assert.AreEqual(_contextCachedItem, _response);
		}

		[Test]
		public void should_NOT_incremment_LongTermCacheMisses()
		{
			Execute();

			Assert.AreEqual(_previousMetrics.LongTermCacheMisses, _metrics.LongTermCacheMisses);
		}

		[Test]
		public void should_NOT_incremment_LongTermCacheHits()
		{
			Execute();

			Assert.AreEqual(_previousMetrics.LongTermCacheHits, _metrics.LongTermCacheHits);
		}

		[Test]
		public void should_NOT_incremment_LongTermCacheLookups()
		{
			Execute();

			Assert.AreEqual(_previousMetrics.LongTermCacheLookUps, _metrics.LongTermCacheLookUps);
		}

		[Test]
		public void should_NOT_incremment_VolatileCacheLookups()
		{
			Execute();

			Assert.AreEqual(_previousMetrics.VolatileCacheLookUps, _metrics.VolatileCacheLookUps);
		}

		[Test]
		public void should_NOT_incremment_VolatileCacheMisses()
		{
			Execute();

			Assert.AreEqual(_previousMetrics.VolatileCacheMisses, _metrics.VolatileCacheMisses);
		}

		[Test]
		public void should_NOT_incremment_VolatileCacheHits()
		{
			Execute();

			Assert.AreEqual(_previousMetrics.VolatileCacheHits, _metrics.VolatileCacheHits);
		}

		private void Execute(IContextCache contextCache = null, IVolatileCache volatileCache = null, ILongTermCache longTermCache = null)
		{
			var cache = TestHelpers.GetCache(_contextCache_Full);

			_response = cache.Get<CachedData>(_lookupKey);
			_metrics = BlendedCacheMetricsStore.GetCacheMetrics().SingleOrDefault(x => _cacheKey.Equals(x.CacheKey, StringComparison.OrdinalIgnoreCase)) ?? new Metrics();
		}
	}
}
