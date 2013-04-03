using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Framework.IntegrationTests.SimpleSetTests
{
	[TestFixture]
	public class SetTest_NoTypeConfiguration
	{
		private const string _cacheKey = "AreYouTheGatekeeper";
		private CachedData _cachedItem;
		private IContextCache _contextCache;
		private IVolatileCache _volatileCache;
		private ILongTermCache _longTermCache;
		private IBlendedCacheConfiguration _configuration;
		
		[SetUp]
		public void SetUp()
		{
			_cachedItem = new CachedData();

			_contextCache = new DictionaryContextCache(_cacheKey, _cachedItem);
			_volatileCache = new DictionaryVolatileCache(_cacheKey, _cachedItem);
			_longTermCache = new DictionaryLongTermCache(_cacheKey, _cachedItem);

			_configuration = new BlendedCacheConfiguration();
		}

		[Test]
		public void when_set_should_set_ContextCache() 
		{
			_contextCache = new DictionaryContextCache(); // remove the item

			Execute();

			Assert.AreEqual(_cachedItem, _contextCache.Get<CachedData>(_cacheKey));
		}

		[Test]
		public void when_set_should_set_VolatileCache()
		{
			_volatileCache = new DictionaryVolatileCache();

			Execute();

			Assert.AreEqual(_cachedItem, _volatileCache.Get<CachedData>(_cacheKey));
		}

		[Test]
		public void when_set_should_set_LongTermCache()
		{
			_longTermCache = new DictionaryLongTermCache();

			Execute();

			Assert.AreEqual(_cachedItem, _longTermCache.Get<CachedData>(_cacheKey));
		}

		[Test]
		public void when_set_should_replace_ContextCache()
		{
			Execute();

			Assert.AreEqual(_cachedItem, _contextCache.Get<CachedData>(_cacheKey));
		}

		[Test]
		public void when_set_should_replace_VolatileCache()
		{
			Execute();

			Assert.AreEqual(_cachedItem, _volatileCache.Get<CachedData>(_cacheKey));
		}

		[Test]
		public void when_set_should_replace_LongTermCache()
		{
			Execute();

			Assert.AreEqual(_cachedItem, _longTermCache.Get<CachedData>(_cacheKey));
		}

		private void Execute()
		{
			var blendedCache = TestHelpers.GetCache(_contextCache, _volatileCache, _longTermCache, _configuration);

			blendedCache.Set(_cacheKey, _cachedItem);
		}
	}
}
