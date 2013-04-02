﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Base class to aid in configuration.  Most configuration operations are supported by creating this object 
	/// and using the protected methods.  This object can be passed into the BlendedCache constructor for advanced operations.
	/// </summary>
	public class BlendedCacheConfiguration : IBlendedCacheConfiguration
	{
		IDictionary<Type, ITypeConfiguration> _typeConfigurations = new Dictionary<Type, ITypeConfiguration>();
		/// <summary>
		/// Creates a default BlendedCacheConfiguration object.
		/// </summary>
		public BlendedCacheConfiguration()
		{
			EnforceAllLoadedTypesAreConfigDefined = false;

		}

		/// <summary>
		/// Will get the type configuration information for the requested type.  If the EnforceAllLoadedTypesAreDefind flag
		/// is set and the type hasn't been defined, this method will throw a BlendedCacheConfigurationException.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public virtual ITypeConfiguration GetTypeConfiguration(Type type)
		{
			ITypeConfiguration config = null;

			//if it found it, doesn't matter, return.
			if (_typeConfigurations.TryGetValue(type, out config))
				return config;

			//if the type definition is required.
			if (EnforceAllLoadedTypesAreConfigDefined)
				throw new BlendedCacheConfigurationException("Missing type: " + type.FullName + " configuration object. Please register this type with the RegisterTypeConfiguration method.");

			//okay to return null
			return null;
		}

		/// <summary>
		/// Will register the TypeConfiguration for the provided type so the blended cache loader can operate on it.
		/// </summary>
		/// <param name="type">The type that is being loaded.  The type being returned from cache.</param>
		/// <param name="configuration"></param>
		public virtual void RegisterTypeConfiguration(Type type, ITypeConfiguration configuration)
		{
			//adding is rare, so no concurrent dictionary here.
			lock (_typeConfigurations)
			{
				_typeConfigurations[type] = configuration;
			}
		}

		/// <summary>
		/// Determines if a prefix should be applied to all cache keys.
		/// </summary>
		public string CacheKeyRoot { get; set; }

		/// <summary>
		/// Contains the default cache timeout structure when a TypeConfiguration is not defined.
		/// </summary>
		public ICacheTimeout DefaultCacheTimeout { get; set; }

		/// <summary>
		/// Will require that all types loaded through the Get method need to have a type loader defined.  Default it false.
		/// </summary>
		public bool EnforceAllLoadedTypesAreConfigDefined { get; set; }

		/// <summary>
		/// Will get the cache configuration for the provided type.  Should return the default cacheTimeout when 
		/// there is no type registered.
		/// </summary>
		/// <param name="type">The type to look for the timeouts on.</param>
		/// <returns>Will return the type specific ICacheTimeout or the DefaultCacheTimeout.</returns>
		public ICacheTimeout GetCacheTimeoutForTypeOrDefault(Type type)
		{
			var config = GetTypeConfiguration(type);

			if (config == null)
				return DefaultCacheTimeout;

			if (config.CacheTimeout == null)
				return DefaultCacheTimeout;

			return config.CacheTimeout;
		}
	}
}