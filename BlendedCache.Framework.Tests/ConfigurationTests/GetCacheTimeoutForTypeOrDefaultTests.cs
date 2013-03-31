using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMM = Rhino.Mocks.MockRepository;
using RME = Rhino.Mocks.RhinoMocksExtensions;

namespace BlendedCache.Tests.ConfigurationTests
{
	[TestFixture]
	public class GetCacheTimeoutForTypeOrDefaultTests
	{
		private BlendedCacheConfiguration<TDataLoaderMock> _configuration;
		private Type _registeredType = null;
		private ITypeConfiguration _registeredConfig = null;
		private Type _lookedUpType = null;
		private ICacheTimeout _response;
		private ICacheTimeout _defaultCacheTimeout;
		private ICacheTimeout _registeredTypeCacheTimeout;

		[SetUp]
		public void SetUp()
		{
			_response = null;
			_registeredType = typeof(TDataMock);
			_registeredTypeCacheTimeout = RMM.GenerateStub<ICacheTimeout>();
			_registeredConfig = RMM.GenerateStrictMock<ITypeConfiguration>();
			RME.Stub(_registeredConfig, x => x.CacheTimeout).Return(_registeredTypeCacheTimeout);
			_lookedUpType = _registeredType;
			_defaultCacheTimeout = RMM.GenerateStrictMock<ICacheTimeout>();
		}

		[Test]
		public void when_type_not_registered_should_return_DefaultCacheTimeout()
		{
			_lookedUpType = typeof(string);

			Execute();

			Assert.AreEqual(_defaultCacheTimeout, _response);
		}

		[Test]
		public void when_type_registered_but_no_CacheTimeout_should_return_DefaultCacheTimeout()
		{
			RME.Stub(_registeredConfig, x => x.CacheTimeout).Return(null).Repeat.Any();

			Execute();

			Assert.AreEqual(_defaultCacheTimeout, _response);
		}

		[Test]
		public void when_type_registered_should_return_CacheTimeout()
		{
			Execute();

			Assert.AreEqual(_registeredTypeCacheTimeout, _response);
		}

		private void Execute()
		{
			_configuration = new BlendedCacheConfiguration<TDataLoaderMock>();
			_configuration.DefaultCacheTimeout = _defaultCacheTimeout;
			_configuration.RegisterTypeConfiguration(_registeredType, _registeredConfig);

			_response = _configuration.GetCacheTimeoutForTypeOrDefault(_lookedUpType);
		}
	}
}