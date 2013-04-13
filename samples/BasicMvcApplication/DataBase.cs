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

		/// <summary>
		/// This simulates getting a list of ids.
		/// </summary>
		/// <returns></returns>
		public static List<SampleData> GetSampleDatas(params int[] ids)
		{
			return ids.Select(GetSampleData).ToList();
		}
	}
}