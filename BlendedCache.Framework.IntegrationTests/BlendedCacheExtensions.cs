using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache.Framework.IntegrationTests
{
	/// <summary>
	/// holds extension methods for ripping into blended cache to check on the internals, not ideal, but actually makes
	/// mocking and testing 1,000 times easier.
	/// </summary>
	internal static class BlendedCacheExtensions
	{
		private const string _contextCachePrivateField = "_contextCache";
		private const string _volatileCachePrivateField = "_volatileCache";
		private const string _longTermCachePrivateField = "_longTermCache";

		/// <summary>
		/// Will get the private field for the context cache.
		/// </summary>
		internal static IContextCache GetContextCache(this BlendedCache blendedCache)
		{
			return blendedCache.GetType().GetField(_contextCachePrivateField, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(blendedCache) as IContextCache;
		}

		/// <summary>
		/// Will get the private field for the volatile cache.
		/// </summary>
		internal static IVolatileCache GetVolatileCache(this BlendedCache blendedCache)
		{
			return blendedCache.GetType().GetField(_volatileCachePrivateField, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(blendedCache) as IVolatileCache;
		}

		/// <summary>
		/// Will get the private field for the volatile cache.
		/// </summary>
		internal static ILongTermCache GetLongTermCache(this BlendedCache blendedCache)
		{
			return blendedCache.GetType().GetField(_longTermCachePrivateField, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(blendedCache) as ILongTermCache;
		}


		/// <summary>
		/// Tests to make sure the fields didn't change to help identify strange problems.
		/// </summary>
		[TestFixture]
		public class BlendedCachePrivateFieldTests
		{

			[Test]
			public void should_have_a_context_cache_field()
			{
				Assert.NotNull(typeof(BlendedCache).GetField(_contextCachePrivateField, BindingFlags.NonPublic | BindingFlags.Instance));
			}

			[Test]
			public void should_have_a_volatile_cache_field()
			{
				Assert.NotNull(typeof(BlendedCache).GetField(_volatileCachePrivateField, BindingFlags.NonPublic | BindingFlags.Instance));
			}

			[Test]
			public void should_have_a_longTerm_cache_field()
			{
				Assert.NotNull(typeof(BlendedCache).GetField(_longTermCachePrivateField, BindingFlags.NonPublic | BindingFlags.Instance));
			}
		}
	}
}