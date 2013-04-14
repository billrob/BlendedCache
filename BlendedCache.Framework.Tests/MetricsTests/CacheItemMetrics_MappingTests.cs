using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Tests.MetricsTests
{
	[TestFixture]
	public class CacheItemMetrics_MappingTests
	{
		private Metrics _public;
		private CachedItemMetrics _private;

		[SetUp]
		public void SetUp()
		{
			_private = new CachedItemMetrics("mycustomkey", new object());
		}

		[Test]
		public void when_mapping_from_CacheItemMetrics_to_public_Metrics_should_map_CacheKey()
		{
			Execute();
			Assert.NotNull(_private.CacheKey);
			Assert.AreEqual(_private.CacheKey, _public.CacheKey);
		}

		[Test]
		public void when_mapping_from_CacheItemMetrics_to_public_Metrics_should_map_LookupKey()
		{
			Execute();
			Assert.NotNull(_private.LookupKey);
			Assert.AreEqual(_private.LookupKey, _public.LookupKey);
		}

		[Test]
		public void when_mapping_from_CacheItemMetrics_to_public_Metrics_should_map_LongTermCacheHits()
		{
			_private.SetValue("_longTermCacheHits", 384729843);

			Execute();

			Assert.AreEqual(_public.LongTermCacheHits, _private.GetValue("_longTermCacheHits"));
			Assert.AreNotEqual(0, _public.LongTermCacheHits);
		}
		[Test]
		public void when_mapping_from_CacheItemMetrics_to_public_Metrics_should_map_LongTermCacheLookUps()
		{
			_private.SetValue("_longTermCacheLookUps", 1);

			Execute();

			Assert.AreEqual(_public.LongTermCacheLookUps, _private.GetValue("_longTermCacheLookUps"));
			Assert.AreNotEqual(0, _private.GetValue("_longTermCacheLookUps"));
		}
		[Test]
		public void when_mapping_from_CacheItemMetrics_to_public_Metrics_should_map_LongTermCacheMisses()
		{
			_private.SetValue("_longTermCacheMisses", 1);

			Execute();

			Assert.AreEqual(_public.LongTermCacheMisses, _private.GetValue("_longTermCacheMisses"));
			Assert.AreNotEqual(0, _private.GetValue("_longTermCacheMisses"));
		}
		[Test]
		public void when_mapping_from_CacheItemMetrics_to_public_Metrics_should_map_TimeInBackgroundLoad()
		{
			_private.SetValue("_timeInBackgroundLoad", 1);

			Execute();

			Assert.AreEqual(_public.TimeInBackgroundLoad, _private.GetValue("_timeInBackgroundLoad"));
			Assert.AreNotEqual(0, _private.GetValue("_timeInBackgroundLoad"));
		}
		[Test]
		public void when_mapping_from_CacheItemMetrics_to_public_Metrics_should_map_TimeInLoad(){
			_private.SetValue("_timeInLoad", 1);

			Execute();

			Assert.AreEqual(_public.TimeInLoad, _private.GetValue("_timeInLoad"));
			Assert.AreNotEqual(0, _private.GetValue("_timeInLoad"));
		}
		[Test]
		public void when_mapping_from_CacheItemMetrics_to_public_Metrics_should_map_TimesBackgroundLoaded()
		{
			_private.SetValue("_timesBackgroundLoaded", 1298798);

			Execute();

			Assert.AreEqual(_public.TimesBackgroundLoaded, _private.GetValue("_timesBackgroundLoaded"));
			Assert.AreNotEqual(0, _private.GetValue("_timesBackgroundLoaded"));
		}
		[Test]
		public void when_mapping_from_CacheItemMetrics_to_public_Metrics_should_map_TimesBackgroundLoadFailed(){
			_private.SetValue("_timesBackgroundLoadFailed", 95686049);

			Execute();

			Assert.AreEqual(_public.TimesBackgroundLoadFailed, _private.GetValue("_timesBackgroundLoadFailed"));
			Assert.AreNotEqual(0, _private.GetValue("_timesBackgroundLoadFailed"));
		}
		[Test]
		public void when_mapping_from_CacheItemMetrics_to_public_Metrics_should_map_TimesFlushed(){
			_private.SetValue("_timesFlushed", 4877953);

			Execute();

			Assert.AreEqual(_public.TimesFlushed, _private.GetValue("_timesFlushed"));
			Assert.AreNotEqual(0, _private.GetValue("_timesFlushed"));
		}
		[Test]
		public void when_mapping_from_CacheItemMetrics_to_public_Metrics_should_map_TimesLoaded()
		{
			_private.SetValue("_timesLoaded", 2398472);

			Execute();
				
			Assert.AreEqual(_public.TimesLoaded, _private.GetValue("_timesLoaded"));
			Assert.AreNotEqual(0, _private.GetValue("_timesLoaded"));
		}
		[Test]
		public void when_mapping_from_CacheItemMetrics_to_public_Metrics_should_map_VolatileCacheHits()
		{
			_private.SetValue("_volatileCacheHits", 659804);

			Execute();

			Assert.AreEqual(_public.VolatileCacheHits, _private.GetValue("_volatileCacheHits"));
			Assert.AreNotEqual(0, _private.GetValue("_volatileCacheHits"));
		}
		[Test]
		public void when_mapping_from_CacheItemMetrics_to_public_Metrics_should_map_VolatileCacheLookUps(){
			_private.SetValue("_volatileCacheLookUps", 238979);

			Execute();

			Assert.AreEqual(_public.VolatileCacheLookUps, _private.GetValue("_volatileCacheLookUps"));
			Assert.AreNotEqual(0, _private.GetValue("_volatileCacheLookUps"));
		}
		[Test]
		public void when_mapping_from_CacheItemMetrics_to_public_Metrics_should_map_VolatileCacheMisses()
		{
			_private.SetValue("_volatileCacheMisses", 2384);

			Execute();

			Assert.AreEqual(_public.VolatileCacheMisses, _private.GetValue("_volatileCacheMisses"));
			Assert.AreNotEqual(0, _private.GetValue("_volatileCacheMisses"));
		}

		[Test]
		public void when_mapping_from_CacheItemMetrics_to_public_Metrics_should_map_DateCreated()
		{
			_private.SetValue("_dateCreated", DateTime.UtcNow.Ticks);

			Execute();

			Assert.AreEqual(_public.DateCreated.Ticks, _private.GetValue("_dateCreated"));
			Assert.AreNotEqual(0, _private.GetValue("_dateCreated"));

		}
		[Test]
		public void when_mapping_from_CacheItemMetrics_to_public_Metrics_should_map_FirstLoaded()
		{
			_private.SetValue("_firstLoaded", DateTime.UtcNow.Ticks);

			Execute();

			Assert.AreEqual(_public.FirstLoaded.Ticks, _private.GetValue("_firstLoaded"));
			Assert.AreNotEqual(0, _private.GetValue("_firstLoaded"));
		}
		[Test]
		public void when_mapping_from_CacheItemMetrics_to_public_Metrics_should_map_LastLoaded()

		{
			_private.SetValue("_lastLoaded", DateTime.UtcNow.Ticks);

			Execute();

			Assert.AreEqual(_public.LastLoaded.Ticks, _private.GetValue("_lastLoaded"));
			Assert.AreNotEqual(0, _private.GetValue("_lastLoaded"));
		}


		

		private void Execute()
		{
			_public = _private.GetMetrics();
		}
	}
}
