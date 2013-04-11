using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicMvcApplication
{
	public static class DataBase
	{
		public static SampleData GetSampleData(int id)
		{
			var data = new SampleData("Name_" + id.ToString(), id);

			return data;
		}
	}
}