namespace Testbed.Dapper.Model
{
	using System;
	using System.Collections;
	using System.Collections.Generic;

	public class Order
	{
		public int OrderID { get; set; }
		public string CustomerID { get; set; }
		public int? EmployeeID { get; set; }
		public DateTime? OrderDate { get; set; }
		public DateTime? RequiredDate { get; set; }
		public DateTime? ShippedDate { get; set; }
		public decimal? Freight { get; set; }
		public string ShipName { get; set; }
		public string ShipAddress { get; set; }
		public string ShipCity { get; set; }
		public string ShipRegion { get; set; }
		public string ShipPostalCode { get; set; }
		public string ShipCountry { get; set; }

		public ICollection<OrderLine> Lines { get; set; }

		//private IList<OrderLine> _lines = new List<OrderLine>();
		//public IEnumerable<OrderLine> Lines { get { return _lines; } }

	}
}
