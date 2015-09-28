namespace Testbed.Dapper.Model
{
	public class OrderLine
	{
		public int OrderID { get; set; }
		public int ProductID { get; set; }
		public decimal UnitPrice { get; set; }
		public short/*QuantityEnumeration*/ Quantity { get; set; }
		public float Discount { get; set; }
	}
}
