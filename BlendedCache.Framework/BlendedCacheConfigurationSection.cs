using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlendedCache
{
	/// <summary>
	/// Represents the configuration section handler for BlendedCache settings.
	/// </summary>
	internal class BlendedCacheConfigurationSection : ConfigurationSection
	{
		/// <summary>
		/// TODO: needs comment for EnforceAllLoadedTypesAreConfigDefined
		/// </summary>
		[ConfigurationProperty("enforceAllLoadedTypesAreConfigDefined", IsRequired = false)]
		public Boolean EnforceAllLoadedTypesAreConfigDefined
		{
			get
			{
				Boolean? result = (Boolean?)this["enforceAllLoadedTypesAreConfigDefined"];
				return result.HasValue ? result.Value : false;
			}
		}
	}
}
