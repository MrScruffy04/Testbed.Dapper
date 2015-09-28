namespace Testbed.Dapper
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Text;

	using global::Dapper;
	using global::DapperExtensions;

	using Testbed.Dapper.Model;
	using Testbed.Dapper.CoreObjects;

	public interface IOrderRepository
	{
		Order GetById(int orderId);
		Order GetById2(int orderId);
		void Save(Order order);
	}

	public class OrderRepository : IOrderRepository
	{
		private readonly Func<IDbConnection> _connectionFactory;

		public OrderRepository(Func<IDbConnection> connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public Order GetById(int orderId)
		{
			Order order = null;

			var gmp = new GetMultiplePredicate();
			gmp.Add<Order>(Predicates.Field<Order>(x => x.OrderID, Operator.Eq, orderId));
			gmp.Add<OrderLine>(Predicates.Field<OrderLine>(x => x.OrderID, Operator.Eq, orderId));

			using (var conn = _connectionFactory())
			{
				var reader = conn.GetMultiple(gmp);

				order = reader.Read<Order>().SingleOrDefault();

				if (order != null)
				{
					//CoreObjectShim.SetReadOnlyProperty<Order>(x => x.Lines, reader.Read<OrderLine>().ToList());
					var setter = MemberShim.GetReadOnlySetter<Order, List<OrderLine>>(x => x.Lines);
					setter(order, reader.Read<OrderLine>().ToList());
				}
			}

			return order;
		}

		public Order GetById2(int orderId)
		{
			string sql = @"
SELECT [OrderID]
      ,[CustomerID]
      ,[EmployeeID]
      ,[OrderDate]
      ,[RequiredDate]
      ,[ShippedDate]
      ,[ShipVia]
      ,[Freight]
      ,[ShipName]
      ,[ShipAddress]
      ,[ShipCity]
      ,[ShipRegion]
      ,[ShipPostalCode]
      ,[ShipCountry]
  FROM [Orders]
 WHERE
      [OrderID] = @orderId;

SELECT d.[OrderID]
      ,d.[ProductID]
      ,d.[UnitPrice]
      ,d.[Quantity]
      ,d.[Discount]
  FROM [Order Details] d
		INNER JOIN [Orders] o ON d.OrderID = o.OrderID
WHERE
		o.OrderID = @orderId;
";

			using (var conn = _connectionFactory())
			{
				var reader = conn.QueryMultiple(sql, new { orderId = orderId });

				var order = reader.Read().SingleOrDefault();
				var lines = reader.Read().ToList();

				if (order != null)
				{
					return new Order
					{
						OrderID = order.OrderID,
						CustomerID = order.CustomerID,
						EmployeeID = order.EmployeeID,
						OrderDate = order.OrderDate,
						RequiredDate = order.RequiredDate,
						ShippedDate = order.ShippedDate,
						Freight = order.Freight,
						ShipName = order.ShipName,
						ShipAddress = order.ShipAddress,
						ShipCity = order.ShipCity,
						ShipRegion = order.ShipRegion,
						ShipPostalCode = order.ShipPostalCode,
						ShipCountry = order.ShipCountry,
						Lines = lines.Select(line => new OrderLine
						{
							Discount = line.Discount,
							OrderID = line.OrderID,
							ProductID = line.ProductID,
							Quantity = line.Quantity,
							UnitPrice = line.UnitPrice,
						}).ToList(),
					};
				}

			}

			return null;
		}

		public Order GetById3(int orderId)
		{
			Order order = null;

			string sql = @"
SELECT [OrderID]
      ,[CustomerID]
      ,[EmployeeID]
      ,[OrderDate]
      ,[RequiredDate]
      ,[ShippedDate]
      ,[ShipVia]
      ,[Freight]
      ,[ShipName]
      ,[ShipAddress]
      ,[ShipCity]
      ,[ShipRegion]
      ,[ShipPostalCode]
      ,[ShipCountry]
  FROM [Orders]
 WHERE
      [OrderID] = @orderId;

SELECT d.[OrderID]
      ,d.[ProductID]
      ,d.[UnitPrice]
      ,d.[Quantity]
      ,d.[Discount]
  FROM [Order Details] d
		INNER JOIN [Orders] o ON d.OrderID = o.OrderID
WHERE
		o.OrderID = @orderId;
";

			using (var conn = _connectionFactory())
			{
				var reader = conn.QueryMultiple(sql, new { orderId = orderId });

				order = reader.Read<Order>().SingleOrDefault();

				if (order != null)
				{
					order.Lines = reader.Read<OrderLine>().ToList();
				}
			}

			return order;
		}

		public void Save(Order order)
		{
			using (var conn = _connectionFactory())
			{
				conn.Update(order);

				foreach (var line in order.Lines)
				{
					conn.Update(line);
				}
			}
		}
	}
}
