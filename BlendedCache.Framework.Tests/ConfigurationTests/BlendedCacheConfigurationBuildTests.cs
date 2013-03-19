using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMM = Rhino.Mocks.MockRepository;

namespace BlendedCache.Tests.ConfigurationTests
{
	[TestFixture]
	public class BlendedCacheConfigurationBuildTests
	{
		private bool _enforce;
		private Dictionary<Type, ITypeConfiguration> _configurationDictionary;
		private Type _typeToLookup;
		private ITypeConfiguration _response;

		[SetUp]
		public void SetUp()
		{
			_response = null;
			_enforce = false;
			_typeToLookup = typeof(BlendedCacheConfigurationBuildTests);

			_configurationDictionary = new Dictionary<Type, ITypeConfiguration>();
			_configurationDictionary.Add(typeof(BlendedCacheConfigurationBuildTests), RMM.GenerateStrictMock<ITypeConfiguration>());
		}

		[Test]
		public void when_type_is_found_should_return_correct_type()
		{
			Execute();

			Assert.NotNull(_response);
			Assert.AreEqual(_configurationDictionary[_typeToLookup], _response);
		}

		[Test]
		public void when_type_is_added_should_return_correct_type()
		{
			var configMock = RMM.GenerateStrictMock<ITypeConfiguration>();
			_typeToLookup = typeof(int);
			_configurationDictionary.Add(typeof(int), configMock);

			Execute();

			Assert.NotNull(_response);
			Assert.AreEqual(configMock, _response);
		}

		[Test]
		public void when_RegisterType_replaces_should_return_new_TypeConfiguration()
		{
			var configMock = RMM.GenerateStrictMock<ITypeConfiguration>();
			_configurationDictionary[_typeToLookup] = configMock;

			Execute();

			Assert.AreEqual(configMock, _response);
		}

		[Test]
		public void when_type_is_not_found_should_return_null()
		{
			_typeToLookup = typeof(int);

			Execute();

			Assert.Null(_response);
		}

		[Test]
		[ExpectedException(typeof(BlendedCacheConfigurationException))]
		public void EnforceConfig_is_true_and_type_is_not_found_should_throw_exception()
		{
			_typeToLookup = typeof(int);
			_enforce = true;

			Execute();
		}

		[Test]
		public void EnforceConfig_is_true_and_type_found_should_return_config()
		{
			_enforce = true;

			Execute();

			Assert.NotNull(_response);
			Assert.AreEqual(_configurationDictionary[_typeToLookup], _response);
		}

		private void Execute()
		{
			var config = new Config(_enforce);

			foreach (var pair in _configurationDictionary)
			{
				config.RegisterTypeConfiguration(pair.Key, pair.Value);
			}

			_response = config.GetTypeConfiguration(_typeToLookup);
		}

		private class Config : BlendedCacheConfiguration<TDataLoaderMock>
		{
			public Config(bool enforce)
			{
				EnforceAllLoadedTypesAreConfigDefined = enforce;
			}
		}
	}
}
