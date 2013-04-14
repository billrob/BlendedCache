using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Framework.IntegrationTests.SimpleGetTests
{
	[TestFixture]
	public class FoundInLongTermCacheTests
	{
		private static Guid _lookupKey = Guid.NewGuid();
		private string _cacheKey;
		private CachedData _cachedItem;
		private CachedData _response;
		private Metrics _previousMetrics;
		private Metrics _metrics;
		private IContextCache _contextCache;
		private IVolatileCache _volatileCache;
		private const int _expirationSeconds = 60;

		[SetUp]
		public void SetUp()
		{
			_contextCache = null;
			_volatileCache = null;

			_cacheKey = new DefaultCacheKeyConverter().ConvertCacheKey<CachedData, Guid>("", _lookupKey);
			_response = null;
			_cachedItem = new CachedData();
			_previousMetrics = BlendedCacheMetricsStore.GetCachedItemMetrics<CachedData, Guid>(_lookupKey) ?? new Metrics();
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
		public void should_incremment_LongTermCacheHits()
		{
			Execute();

			Assert.AreEqual(_previousMetrics.LongTermCacheHits + 1, _metrics.LongTermCacheHits);
		}

		[Test]
		public void should_incremment_LongTermCacheLookups()
		{
			Execute();

			Assert.AreEqual(_previousMetrics.LongTermCacheLookUps + 1, _metrics.LongTermCacheLookUps);
		}

		[Test]
		public void should_incremment_VolatileCacheLookups()
		{
			Execute();

			Assert.AreEqual(_previousMetrics.VolatileCacheLookUps + 1, _metrics.VolatileCacheLookUps);
		}

		[Test]
		public void should_incremment_VolatileCacheMisses()
		{
			Execute();

			Assert.AreEqual(_previousMetrics.VolatileCacheMisses + 1, _metrics.VolatileCacheMisses);
		}

		[Test]
		public void should_NOT_incremment_VolatileCacheHits()
		{
			Execute();

			Assert.AreEqual(_previousMetrics.VolatileCacheHits, _metrics.VolatileCacheHits);
		}

		[Test]
		public void should_set_CachedItem_on_ContextCache()
		{
			Execute();

			Assert.NotNull(_cachedItem);
			Assert.AreEqual(_cachedItem, _contextCache.Get<CachedData>(_cacheKey));
		}

		[Test]
		public void should_set_CachedItem_on_VolatileCache()
		{
			Execute();

			Assert.NotNull(_cachedItem);
			Assert.AreEqual(_cachedItem, _volatileCache.Get<CachedData>(_cacheKey).CachedItem);
		}

		[Test]
		public void should_set_ExpirationDateTimeUtc_on_VolatileCache()
		{
			Execute();

			Assert.Less(DateTime.UtcNow, _volatileCache.Get<CachedData>(_cacheKey).ExpirationDateTimeUtc);
		}

		private void Execute()
		{
			var longTermCache = new DictionaryLongTermCache(_cacheKey, _cachedItem);
			var cache = TestHelpers.GetCache(longTermCache: longTermCache);

			_contextCache = cache.GetContextCache();
			_volatileCache = cache.GetVolatileCache();

			_response = cache.Get<CachedData, Guid>(_lookupKey);

			_metrics = BlendedCacheMetricsStore.GetCachedItemMetrics<CachedData, Guid>(_lookupKey) ?? new Metrics();
		}
	}
}
