using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMM = Rhino.Mocks.MockRepository;
using RME = Rhino.Mocks.RhinoMocksExtensions;
using NUnit.Framework;

namespace BlendedCache.Tests.BlendedCacheTests.GetTests
{
	[TestFixture]
	public class Get_NoTypeConfigurationTests
	{
		private TDataMock _response;
		private string _cacheKey;
		private ICacheKeyFixupProvider _cacheKeyFixUpProviderMock;
		private IBlendedCacheConfiguration _configurationMock;
		private string _fixedUpCacheKey;
		private bool? _flushMode;

		private CacheItemMetrics _cacheItemMetrics;
		
		private IContextCacheLookup _contextCacheLookupMock;
		private TDataMock _contextCachedObject;

		private IVolatileCacheLookup _volatileCacheLookupMock;
		private TDataMock _volatileCachedObject;

		private ILongTermCacheLookup _longTermCacheLookupMock;
		private TDataMock _longTermCachedObject;

		[SetUp]
		public void SetUp()
		{
			_response = null;
			_cacheKey = "my cache key";
			_fixedUpCacheKey = "FixedUp " + _cacheKey;
			_flushMode = null;

			_configurationMock = null;

			_cacheItemMetrics = new CacheItemMetrics(_fixedUpCacheKey);

			_cacheKeyFixUpProviderMock = RMM.GenerateStrictMock<ICacheKeyFixupProvider>();
			RME.Stub(_cacheKeyFixUpProviderMock, x => x.FixUpCacheKey(null, _cacheKey)).Do(new Func<string, string, string>((a, b) => _fixedUpCacheKey));

			_contextCachedObject = null;
			_contextCacheLookupMock = RMM.GenerateStrictMock<IContextCacheLookup>();
			RME.Stub(_contextCacheLookupMock, x => x.GetDataFromContextCache<TDataMock>(_fixedUpCacheKey)).Do(new Func<string, TDataMock>((a) => _contextCachedObject));

			_volatileCachedObject = null;
			_volatileCacheLookupMock = RMM.GenerateStrictMock<IVolatileCacheLookup>();
			RME.Stub(_volatileCacheLookupMock, x=>x.GetDataFromVolatileCache<TDataMock>(_fixedUpCacheKey, _cacheItemMetrics)).Do(new Func<string, CacheItemMetrics, TDataMock>((a, b) => _volatileCachedObject));

			_longTermCachedObject = null;
			_longTermCacheLookupMock = RMM.GenerateStrictMock<ILongTermCacheLookup>();
			RME.Stub(_longTermCacheLookupMock, x => x.GetDataFromLongTermCache<TDataMock>(_fixedUpCacheKey, _cacheItemMetrics)).Do(new Func<string, CacheItemMetrics, TDataMock>((a, b) => _longTermCachedObject));
		}

		[Test]
		public void should_always_call_FixUpCacheKey()
		{
			Execute();

			RME.AssertWasCalled(_cacheKeyFixUpProviderMock, x => x.FixUpCacheKey(null, _cacheKey), opt => opt.Repeat.Once());
		}

		[Test]
		public void should_always_pass_CacheKeyRoot_to_FixUpCacheKey()
		{
			var cacheKeyRoot = "root";
			_configurationMock = RMM.GenerateStrictMock<IBlendedCacheConfiguration>();
			RME.Stub(_configurationMock, x => x.CacheKeyRoot).Return(cacheKeyRoot);
			RME.Stub(_cacheKeyFixUpProviderMock, x => x.FixUpCacheKey(cacheKeyRoot, _cacheKey)).Return(_fixedUpCacheKey);

			Execute();

			RME.AssertWasCalled(_cacheKeyFixUpProviderMock, x => x.FixUpCacheKey(cacheKeyRoot, _cacheKey), opt => opt.Repeat.Once());
		}

		[Test]
		public void should_always_call_IContextCache_GetDataFromContextCache()
		{
			Execute();

			RME.AssertWasCalled(_contextCacheLookupMock, x => x.GetDataFromContextCache<TDataMock>(_fixedUpCacheKey), opt => opt.Repeat.Once());

		}

		[Test]
		public void when_found_in_IContextCache_should_return_cachedItem()
		{
			_contextCachedObject = new TDataMock();

			Execute();

			Assert.AreEqual(_contextCachedObject, _response);
		}

		[Test]
		public void when_found_in_IContextCache_and_FlushMode_should_return_cachedItem()
		{
			_flushMode = true;
			_contextCachedObject = new TDataMock();

			Execute();

			Assert.AreEqual(_contextCachedObject, _response);
		}

		[Test]
		public void when_found_in_IContextCache_should_not_call_IVolatileCacheLookup()
		{
			_contextCachedObject = new TDataMock();

			Execute();

			RME.AssertWasNotCalled(_volatileCacheLookupMock, x => x.GetDataFromVolatileCache<TDataMock>(null, null), opt => opt.IgnoreArguments());
		}

		[Test]
		public void when_found_in_IContextCache_should_not_call_ILongTermCacheLookup()
		{
			_contextCachedObject = new TDataMock();

			Execute();

			RME.AssertWasNotCalled(_longTermCacheLookupMock, x => x.GetDataFromLongTermCache<TDataMock>(null, null), opt => opt.IgnoreArguments());
		}

		[Test]
		public void when_not_found_in_ContextCache_and_FlushMode_should_not_call_IVolatileCacheLookup()
		{
			_flushMode = true;

			Execute();

			RME.AssertWasNotCalled(_volatileCacheLookupMock, x => x.GetDataFromVolatileCache<TDataMock>(null, null), opt => opt.IgnoreArguments());
		}

		[Test]
		public void when_not_found_in_ContextCache_and_FlushMode_should_not_call_LongTermCacheLookup()
		{
			_flushMode = true;

			Execute();

			RME.AssertWasNotCalled(_longTermCacheLookupMock, x => x.GetDataFromLongTermCache<TDataMock>(null,null), opt => opt.IgnoreArguments());
		}

		[Test]
		public void when_found_in_IVolatileCache_should_not_call_ILongTermCacheLook()
		{
			_volatileCachedObject = new TDataMock();

			Execute();

			RME.AssertWasNotCalled(_longTermCacheLookupMock, x => x.GetDataFromLongTermCache<TDataMock>(null,null), opt => opt.IgnoreArguments());
		}

		[Test]
		public void when_found_in_IVolatileCache_should_return_cachedItem()
		{
			_volatileCachedObject = new TDataMock();

			Execute();

			Assert.AreEqual(_volatileCachedObject, _response);
		}

		[Test]
		public void when_not_found_in_ILongTermCache_should_return_null()
		{
			Execute();

			Assert.Null(_response);
		}

		[Test]
		public void when_found_in_ILongTermCache_should_return_cachedItem()
		{
			_longTermCachedObject = new TDataMock();

			Execute();

			Assert.AreEqual(_longTermCachedObject, _response);
		}

		private void Execute()
		{
			var cache = new BlendedCache(null, null, null, _configurationMock);

			cache.SetService<IContextCacheLookup>(_contextCacheLookupMock); 
			cache.SetService<IVolatileCacheLookup>(_volatileCacheLookupMock);
			cache.SetService<ILongTermCacheLookup>(_longTermCacheLookupMock);
			cache.SetService<ICacheKeyFixupProvider>(_cacheKeyFixUpProviderMock);

			var cacheItemMetricLookup = RMM.GenerateStrictMock<ICacheMetricsLookup>();
			RME.Stub(cacheItemMetricLookup, x => x.GetOrCreateCacheItemMetric(_fixedUpCacheKey)).Return(_cacheItemMetrics);
			cache.SetService<ICacheMetricsLookup>(cacheItemMetricLookup);

			if(_flushMode.HasValue)
				cache.SetFlushMode(_flushMode.Value);

			_response = cache.Get<TDataMock>(_cacheKey);
		}
	}
}