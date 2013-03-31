using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMM = Rhino.Mocks.MockRepository;
using RME = Rhino.Mocks.RhinoMocksExtensions;
using NUnit.Framework;

namespace BlendedCache.Tests.BlendedCacheSetterTests
{
	[TestFixture]
	public class SetTests
	{
		private string _cacheKey;
		private TDataMock _cachedItem;
		private ICacheTimeout _cacheTimeout;
		private SetCacheLocation _location;
		private IContextCache _contextCacheMock;
		private IVolatileCache _volatileCacheMock;
		private ILongTermCache _longTermCacheMock;

		[SetUp]
		public void SetUp()
		{
			_location = SetCacheLocation.NotSet;
			_cacheKey = "my cacheKey";
			_cachedItem = new TDataMock();
			_cacheTimeout = new CacheTimeoutMock()
			{
				LongTermRefreshInSeconds = 32834,
				LongTermTimeoutInSeconds = 124343,
				VolatileTimeoutInSeconds = 12823,
			};

			_contextCacheMock = RMM.GenerateStrictMock<IContextCache>();
			RME.Stub(_contextCacheMock, x => x.Set<TDataMock>(_cacheKey, _cachedItem));

			_volatileCacheMock = RMM.GenerateStrictMock<IVolatileCache>();
			RME.Stub(_volatileCacheMock, x => x.Set<TDataMock>(_cacheKey, _cachedItem, _cacheTimeout.VolatileTimeoutInSeconds));

			_longTermCacheMock = RMM.GenerateStrictMock<ILongTermCache>();
			RME.Stub(_longTermCacheMock, x => x.Set<TDataMock>(_cacheKey, _cachedItem, _cacheTimeout.LongTermRefreshInSeconds, _cacheTimeout.LongTermTimeoutInSeconds));
		}

		[Test]
		public void when_CacheLocation_is_NotSet_should_not_call_anything()
		{
			_contextCacheMock = RMM.GenerateStrictMock<IContextCache>();
			_volatileCacheMock = RMM.GenerateStrictMock<IVolatileCache>();
			_longTermCacheMock = RMM.GenerateStrictMock<ILongTermCache>();

			_location = SetCacheLocation.NotSet;

			Execute();

			//the fact it doesn't thrown an exception means it wasn't called.
		}

		[Test]
		public void when_CacheLocation_is_Context_should_set_ContextCache()
		{
			_volatileCacheMock = RMM.GenerateStrictMock<IVolatileCache>();
			_longTermCacheMock = RMM.GenerateStrictMock<ILongTermCache>();

			_location = SetCacheLocation.ContextCache;

			Execute();

			RME.AssertWasCalled(_contextCacheMock, x => x.Set<TDataMock>(_cacheKey, _cachedItem), opt => opt.Repeat.Once());
		}

		[Test]
		public void when_CacheLocation_is_Volatile_should_set_ContextCache()
		{
			_location = SetCacheLocation.VolatileCache;

			Execute();

			RME.AssertWasCalled(_contextCacheMock, x => x.Set<TDataMock>(_cacheKey, _cachedItem), opt => opt.Repeat.Once());
		}

		[Test]
		public void when_CacheLocation_is_Volatile_should_set_VolatileCache()
		{
			_location = SetCacheLocation.VolatileCache;

			Execute();

			RME.AssertWasCalled(_volatileCacheMock, x => x.Set<TDataMock>(_cacheKey, _cachedItem, _cacheTimeout.VolatileTimeoutInSeconds), opt => opt.Repeat.Once());
		}

		[Test]
		public void when_CacheLocation_is_LongTerm_should_set_ContextCache()
		{
			_location = SetCacheLocation.LongTermCache;

			Execute();

			RME.AssertWasCalled(_contextCacheMock, x => x.Set<TDataMock>(_cacheKey, _cachedItem), opt => opt.Repeat.Once());
		}

		[Test]
		public void when_CacheLocation_is_LongTerm_should_set_VolatileCache()
		{
			_location = SetCacheLocation.LongTermCache;

			Execute();

			RME.AssertWasCalled(_volatileCacheMock, x => x.Set<TDataMock>(_cacheKey, _cachedItem, _cacheTimeout.VolatileTimeoutInSeconds), opt => opt.Repeat.Once());
		}

		[Test]
		public void when_CacheLocation_is_LongTerm_should_set_LongTermCache()
		{
			_location = SetCacheLocation.LongTermCache;

			Execute();

			RME.AssertWasCalled(_longTermCacheMock, x => x.Set<TDataMock>(_cacheKey, _cachedItem, _cacheTimeout.LongTermRefreshInSeconds, _cacheTimeout.LongTermTimeoutInSeconds), opt => opt.Repeat.Once());
		}


		private void Execute()
		{
			var setter = new DefaultBlendedCacheSetter() as IBlendedCacheSetter;

			setter.Set(_cacheKey, _cachedItem, _cacheTimeout, _location, _contextCacheMock, _volatileCacheMock, _longTermCacheMock);
		}
	}
}