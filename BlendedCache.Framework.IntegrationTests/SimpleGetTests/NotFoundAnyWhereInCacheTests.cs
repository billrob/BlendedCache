using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Framework.IntegrationTests.SimpleGetTests
{
	[TestFixture]
	public class NotFoundAnyWhereInCacheTests
	{
		private const string _cacheKey = "myKey";
		private CachedData _response;
		private Metrics _previousMetrics;
		private Metrics _metrics;
		
		[SetUp]
		public void SetUp()
		{
			_response = null;
			_previousMetrics = BlendedCacheMetricsStore.GetCacheMetrics().SingleOrDefault(x => _cacheKey.Equals(x.CacheKey, StringComparison.OrdinalIgnoreCase)) ?? new Metrics();
		}

		[Test]
		public void when_not_in_cache_should_return_null()
		{
			Execute();

			Assert.Null(_response);
		}

		[Test]
		public void should_create_metrics()
		{
			Execute();

			Assert.NotNull(_metrics);
		}

		[Test]
		public void should_incremment_LongTermCacheMisses()
		{
			Execute();

			Assert.AreEqual(_previousMetrics.LongTermCacheMisses + 1, _metrics.LongTermCacheMisses);
		}

		[Test]
		public void should_NOT_incremment_LongTermCacheHits()
		{
			Execute();

			Assert.AreEqual(_previousMetrics.LongTermCacheHits, _metrics.LongTermCacheHits);
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

		private void Execute()
		{
			var cache = TestHelpers.GetCache();

			_response = cache.Get<CachedData>(_cacheKey);

			_metrics = BlendedCacheMetricsStore.GetCacheMetrics().SingleOrDefault(x => _cacheKey.Equals(x.CacheKey, StringComparison.OrdinalIgnoreCase));
		}
	}
}
