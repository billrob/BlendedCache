using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicMvcApplication
{
	public class SampleData
	{
		public SampleData(string name, int id = 42)
		{
			Name = name;
			DateCreated = DateTime.UtcNow;
			Id = 42;
		}

		public int Id { get; private set; }
		public string Name { get; private set; }
		public DateTime DateCreated { get; private set; }
	}
}