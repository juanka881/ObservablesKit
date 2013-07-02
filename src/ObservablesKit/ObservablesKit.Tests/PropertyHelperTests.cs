using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ObservablesKit.Tests
{
	[TestFixture]
    public class PropertyHelperTests
    {
		[Test]
		public void can_get_property_path_stack()
		{
			var s = PropertyHelper.GetPropertyPathStack<Person, object>(x => x.Address.Building.Owner.Name.Length);
			var s2 = PropertyHelper.GetPropertyPathStack<Person, object>(x => x);
			Console.WriteLine(s);
		}
    }
}
