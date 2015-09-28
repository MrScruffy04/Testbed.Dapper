namespace Testbed.Dapper
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;

	using System.Configuration;

	using global::Dapper;
	using global::DapperExtensions;
	using global::DapperExtensions.Mapper;

	using Testbed.Dapper.Model;

    class Program
    {
        #region Main Program Loop

        private static ManualResetEvent _quitEvent = new ManualResetEvent(false);

        [STAThread]
        private static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, e) =>
            {
                _quitEvent.Set();
                e.Cancel = true;
            };

            try
            {
                #region Setup

				SqlMapper.AddTypeHandler(new EnumerationTypeHandler<QuantityEnumeration, short>());

                #endregion


                ProgramBody();

                //  One of the following should be commented out. The other should be uncommented.

                //_quitEvent.WaitOne();  //  Wait on UI thread for Ctrl + C

                Console.ReadKey(true);  //  Wait for any character input
            }
            finally
            {
                #region Tear down
                #endregion
            }
        }

        #endregion




		//  ConnectionString name from site settings
		private const string DbConnName = "Northwind";

        private static void ProgramBody()
        {
			var repo = new OrderRepository(() => GetDbConn());

			SimulateRequest(repo, 10284);
		}

		private static void SimulateRequest(IOrderRepository orderRepository, int orderId)
		{
			//  Repo.GetById()
			var order = orderRepository.GetById2(orderId);

			if (order == null)
			{
				Console.WriteLine("No dice");
				return;
			}

			Console.WriteLine(order.ShipAddress);

			foreach (var line in order.Lines)
			{
				Console.WriteLine("\t{0}\t{1}", line.ProductID, line.UnitPrice);
			}


			
#if SaveTest
			//  App logic
			order.RequiredDate = DateTime.UtcNow;

			foreach (var line in order.Lines)
			{
				//line.Quantity++;
			}

			//  Repo.Save()
			orderRepository.Save(order);
#endif
		}

		private static IDbConnection GetDbConn()
		{
			var conn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings[DbConnName].ConnectionString);

			conn.Open();

			return conn;
		}

    }


	public class OrderMapper : ClassMapper<Order>
	{
		public OrderMapper()
		{
			Table("Orders");

			Map(x => x.OrderID)
				.Key(KeyType.Identity);

			Map(x => x.Lines)
				.Ignore();

			base.AutoMap();
		}
	}

	public class OrderLineMapper : ClassMapper<OrderLine>
	{
		public OrderLineMapper()
		{
			Table("Order Details");

			Map(x => x.OrderID)
				.Key(KeyType.Assigned);
			Map(x => x.ProductID)
				.Key(KeyType.Assigned);

			base.AutoMap();
		}
	}

}
