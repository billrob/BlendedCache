using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Tests.MetricsTests
{
	[TestFixture]
	public class BlendedCacheMetricsStoreTests
	{
		[SetUp]
		public void SetUp()
		{
			BlendedCacheMetricsStore.RegisterType<MyType1>();
			BlendedCacheMetricsStore.RegisterType<MyType2>();
		}

		[Test]
		public void when_RegisterType_is_called_should_come_back_in_GetRegisteredTypes()
		{
			Assert.Contains(typeof(MyType1), BlendedCacheMetricsStore.GetRegisteredTypes());
			Assert.Contains(typeof(MyType2), BlendedCacheMetricsStore.GetRegisteredTypes());
		}

		[Test]
		[ExpectedException(typeof(NotImplementedException))]
		public void when_GetCacheMetrics_called_and_does_not_exist_should_throw_NotImplementedException()
		{
			BlendedCacheMetricsStore.GetCacheMetrics(typeof(MyType3_NotRegistered));
		}

		[Test]
		public void when_GetCacheMetrics_called_should_return_list()
		{
			Assert.NotNull(BlendedCacheMetricsStore.GetCacheMetrics(typeof(MyType1)));
			Assert.NotNull(BlendedCacheMetricsStore.GetCacheMetrics(typeof(MyType2)));
		}

		private class MyType1 { }
		private class MyType2 { }
		private class MyType3_NotRegistered { }
	}
}
