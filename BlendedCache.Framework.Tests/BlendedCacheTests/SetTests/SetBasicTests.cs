using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMM = Rhino.Mocks.MockRepository;
using RME = Rhino.Mocks.RhinoMocksExtensions;
using NUnit.Framework;

namespace BlendedCache.Tests.BlendedCacheTests.SetTests
{
	[TestFixture]
	public class SetBasicTests
	{
		private IBlendedCache _blendedCache;
		private TDataMock _cachedItem;
		private string _cacheKey;

		private IContextCache _contextCacheMock;
		private IVolatileCache _volatileCacheMock;
		private ILongTermCache _longTermCacheMock;
		private IBlendedCacheConfiguration _configurationMock;
		private ICacheTimeout _cacheTimeout;

		private ICacheSetter _setterMock;

		private string _passedCacheKey;
		private TDataMock _passedCachedItem;
		private SetCacheLocation _passedCacheLocation;
		private ICacheTimeout _passedCacheTimeout;
		private IContextCache _passedContextCache;
		private IVolatileCache _passedVolatileCache;
		private ILongTermCache _passedLongTermCache;

		[SetUp]
		public void SetUp()
		{
			_cachedItem = new TDataMock();
			_cacheKey = "my cache key";

			_cacheTimeout = RMM.GenerateStrictMock<ICacheTimeout>();

			_contextCacheMock = RMM.GenerateStrictMock<IContextCache>();
			_volatileCacheMock = RMM.GenerateStrictMock<IVolatileCache>();
			_longTermCacheMock = RMM.GenerateStrictMock<ILongTermCache>();

			_configurationMock = RMM.GenerateStub<IBlendedCacheConfiguration>();
			RME.Stub(_configurationMock, x => x.GetCacheTimeoutForTypeOrDefault(_cachedItem.GetType())).Return(_cacheTimeout);

			_setterMock = RMM.GenerateStrictMock<ICacheSetter>();
			RME.Stub(_setterMock, x => x.Set<TDataMock>(null, null, null, SetCacheLocation.NotSet, null, null, null)).IgnoreArguments()
				.Do(new Action<string, TDataMock, ICacheTimeout, SetCacheLocation, IContextCache, IVolatileCache, ILongTermCache>(
					(passedCacheKey, passedCachedItem, passedTimeout, passedLocation, passedContext, passedVolatile, passedLongTerm) =>
					{
						_passedCacheKey = passedCacheKey;
						_passedCachedItem = passedCachedItem;
						_passedCacheTimeout = passedTimeout;
						_passedCacheLocation = passedLocation;
						_passedContextCache = passedContext;
						_passedVolatileCache = passedVolatile; 
						_passedLongTermCache = passedLongTerm;
					}));
		}

		[Test]
		public void should_call_IBlendedCacheConfiguration_GetCacheTimeoutForTypeOrDefault()
		{
			Execute();

			RME.AssertWasCalled(_configurationMock, x => x.GetCacheTimeoutForTypeOrDefault(_cachedItem.GetType()), opt => opt.Repeat.Once());
		}

		[Test]
		public void should_call_IBlendedCacheSetter_Set_method()
		{
			Execute();

			RME.AssertWasCalled(_setterMock, x => x.Set<TDataMock>(_cacheKey, _cachedItem, _cacheTimeout, SetCacheLocation.LongTermCache, _contextCacheMock, _volatileCacheMock, _longTermCacheMock), opt => opt.Repeat.Once());
		}

		[Test]
		public void should_pass_CacheKey()
		{
			Execute();

			Assert.NotNull(_passedCacheKey);
			Assert.AreEqual(_passedCacheKey, _cacheKey);
		}

		[Test]
		public void should_pass_CachedItem()
		{
			Execute();

			Assert.NotNull(_passedCachedItem);
			Assert.AreEqual(_passedCachedItem, _cachedItem);
		}

		[Test]
		public void should_pass_CacheLocation_as_LongTerm()
		{
			Execute();

			Assert.NotNull(_passedCacheLocation);
			Assert.AreEqual(_passedCacheLocation, SetCacheLocation.LongTermCache);
		}

		[Test]
		public void should_pass_CacheTimeout()
		{
			Execute();

			Assert.NotNull(_passedCacheTimeout);
			Assert.AreEqual(_passedCacheTimeout, _cacheTimeout);
		}

		[Test]
		public void should_pass_ContextCache()
		{
			Execute();

			Assert.NotNull(_passedContextCache);
			Assert.AreEqual(_passedContextCache, _contextCacheMock);
		}

		[Test]
		public void should_pass_VolatileCache()
		{
			Execute();

			Assert.NotNull(_passedVolatileCache);
			Assert.AreEqual(_passedVolatileCache, _volatileCacheMock);
		}

		[Test]
		public void should_pass_LongTermCache()
		{
			Execute();

			Assert.NotNull(_passedLongTermCache);
			Assert.AreEqual(_passedLongTermCache, _longTermCacheMock);
		}

		[Test]
		public void should_set_location_to_be_LongTerm()
		{
			Execute();

			Assert.AreEqual(SetCacheLocation.LongTermCache, _passedCacheLocation);
		}

		

		private void Execute()
		{
			var cached = new BlendedCache(_contextCacheMock, _volatileCacheMock, _longTermCacheMock, _configurationMock);
			cached.SetService<ICacheSetter>(_setterMock);

			_blendedCache = cached;
			_blendedCache.Set(_cacheKey, _cachedItem);
		}
	}
}
