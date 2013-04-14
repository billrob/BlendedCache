using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Framework.IntegrationTests.SimpleGetTests
{
	[TestFixture]
	public class FoundInVolatileCacheTests
	{
		public const string _lookupKey = "userKey";
		private string _cacheKey;
		private CachedData _cachedItem;
		private CachedData _response;
		private Metrics _previousMetrics;
		private Metrics _metrics;
		private IContextCache _contextCache;

		[SetUp]
		public void SetUp()
		{
			_cacheKey = new DefaultCacheKeyConverter().ConvertCacheKey<CachedData, string>("", _lookupKey);
			_contextCache = null;
			_response = null;
			_cachedItem = new CachedData();
			_previousMetrics = BlendedCacheMetricsStore.GetCachedItemMetrics<CachedData, string>(_lookupKey) ?? new Metrics();
		}

		[Test]
		public void when_in_cache_should_return_CachedItem()
		{
			Execute();

			Assert.NotNull(_response);
			Assert.AreEqual(_cachedItem, _response);
		}

		[Test]
		public void should_create_metrics()
		{
			Execute();

			Assert.NotNull(_metrics);
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
		public void should_incremment_VolatileCacheLookups()
		{
			Execute();

			Assert.AreEqual(_previousMetrics.VolatileCacheLookUps + 1, _metrics.VolatileCacheLookUps);
		}

		[Test]
		public void should_NOT_incremment_VolatileCacheMisses()
		{
			Execute();

			Assert.AreEqual(_previousMetrics.VolatileCacheMisses, _metrics.VolatileCacheMisses);
		}

		[Test]
		public void should_incremment_VolatileCacheHits()
		{
			Execute();

			Assert.AreEqual(_previousMetrics.VolatileCacheHits + 1, _metrics.VolatileCacheHits);
		}

		[Test]
		public void should_set_CachedItem_on_ContextCache()
		{
			Execute();

			Assert.NotNull(_cachedItem);
			Assert.AreEqual(_cachedItem, _contextCache.Get<CachedData>(_cacheKey));
		}

		private void Execute()
		{
			var volatileCache = new DictionaryVolatileCache(_cacheKey, _cachedItem);
			var cache = TestHelpers.GetCache(volatileCache: volatileCache);

			_contextCache = cache.GetContextCache();
		
			_response = cache.Get<CachedData>(_lookupKey);

			_metrics = BlendedCacheMetricsStore.GetCachedItemMetrics<CachedData, string>(_lookupKey) ?? new Metrics();
		}
	}
}
