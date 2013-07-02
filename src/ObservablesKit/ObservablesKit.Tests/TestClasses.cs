using System.Text;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ObservablesKit.Tests
{
	public class Person
	{
		public Address Address { get; set; }
	}

	public class Address
	{
		public Building Building { get; set; }
	}

	public class Building
	{
		public Owner Owner { get; set; }
	}

	public class Owner
	{
		public string Name { get; set; }
	}
}