using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Framework.IntegrationTests.SimpleGetSetTests
{
	/// <summary>
	/// Will test out all the various in context, not in volatile, int long term get cases to 
	/// ensure the right data item is being returned with flush mode being on.
	/// </summary>
	[TestFixture]
	public class SimpleGetAllVariations_ReturnTypeOnlyTests_FlushMode
	{
		private string _cacheKey = null;
		private CachedData _contextCachedItem = null;
		private CachedData _volatileCachedItem = null;
		private CachedData _longTermCachedItem = null;
		private CachedData _response;
		private IContextCache _contextCache_Empty;
		private IContextCache _contextCache_Full;
		private IVolatileCache _volatileCache_Empty;
		private IVolatileCache _volatileCache_Full;
		private ILongTermCache _longTermCache_Empty;
		private ILongTermCache _longTermCache_Full;
		
		[SetUp]
		public void SetUp()
		{
			_cacheKey = "myKey";
			_response = null;
			_contextCachedItem = new CachedData();
			_volatileCachedItem = new CachedData();
			_longTermCachedItem = new CachedData();
			
			_contextCache_Empty = new DictionaryContextCache();
			_contextCache_Full = new DictionaryContextCache(_cacheKey, _contextCachedItem);

			_volatileCache_Empty = new DictionaryVolatileCache();
			_volatileCache_Full = new DictionaryVolatileCache(_cacheKey, _volatileCachedItem);

			_longTermCache_Empty = new DictionaryLongTermCache();
			_longTermCache_Full = new DictionaryLongTermCache(_cacheKey, _longTermCachedItem);
		}

		[Test]
		public void when_empty_should_return_null()
		{
			Execute();
			
			Assert.Null(_response);
		}

		[Test]
		public void when_in_ContextCache_should_return_ContextItem()
		{
			Execute(contextCache: _contextCache_Full);

			Assert.NotNull(_response);
			Assert.AreEqual(_contextCachedItem, _response);
		}

		[Test]
		public void when_in_VolatileCache_should_return_null()
		{
			Execute(volatileCache: _volatileCache_Full);

			Assert.Null(_response);
		}

		[Test]
		public void when_expired_in_Volatile_should_return_null()
		{
			_cacheKey = _cacheKey + "v1";
			_volatileCache_Full.Set(_cacheKey, new DefaultVolatileCacheEntry<CachedData>(_volatileCachedItem, 0));

			Execute(volatileCache: _volatileCache_Full);

			Assert.Null(_response);
		}

		[Test]
		public void when_in_VolatileCache_and_ContextCache_should_return_ContextItem()
		{
			Execute(contextCache: _contextCache_Full, volatileCache: _volatileCache_Full);

			Assert.NotNull(_response);
			Assert.AreEqual(_contextCachedItem, _response);
		}

		[Test]
		public void when_in_LongTermCache_should_return_null()
		{
			Execute(longTermCache: _longTermCache_Full);

			Assert.Null(_response);
		}

		[Test]
		public void when_in_LongTermCache_and_VolatileCache_should_return_null()
		{
			Execute(volatileCache: _volatileCache_Full, longTermCache: _longTermCache_Full);

			Assert.Null(_response);
		}

		[Test]
		public void when_in_LongTermCache_and_ContextCache_should_return_ContextItem()
		{
			Execute(contextCache: _contextCache_Full, longTermCache: _longTermCache_Full);

			Assert.NotNull(_response);
			Assert.AreEqual(_contextCachedItem, _response);
		}

		[Test]
		public void when_in_LongTermCache_and_VolatileCache_and_ContextCache_should_return_ContextItem()
		{
			Execute(contextCache: _contextCache_Full, volatileCache: _volatileCache_Full, longTermCache: _longTermCache_Full);

			Assert.NotNull(_response);
			Assert.AreEqual(_contextCachedItem, _response);
		}

		private void Execute(IContextCache contextCache = null, IVolatileCache volatileCache = null, ILongTermCache longTermCache = null)
		{
			var cache = TestHelpers.GetCache(contextCache, volatileCache, longTermCache, initialFlushMode: true);

			_response = cache.Get<CachedData>(_cacheKey);
		}
		
	}
}
