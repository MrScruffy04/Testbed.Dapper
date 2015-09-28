namespace Testbed.Dapper.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	//  Custom types like Enumeration are mapped using a SqlMapper.TypeHandler<T>. Use EnumerationTypeHandler<T, TId> for all enumerations. Be sure to register the handler(s) with SqlMapper.AddTypeHandler()
	public class QuantityEnumeration : Enumeration<QuantityEnumeration, short>
	{
		public static QuantityEnumeration None = new QuantityEnumeration(0, "None");
		public static QuantityEnumeration Six = new QuantityEnumeration(6, "Six");
		public static QuantityEnumeration Sixteen = new QuantityEnumeration(16, "Sixteen");
		public static QuantityEnumeration TwentyOne = new QuantityEnumeration(21, "TwentyOne");
		public static QuantityEnumeration TwentyTwo = new QuantityEnumeration(22, "TwentyTwo");

		private QuantityEnumeration(short id, string name)
			: base(id, name)
		{
		}
	}
}
