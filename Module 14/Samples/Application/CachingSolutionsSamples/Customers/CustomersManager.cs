using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;

namespace CachingSolutionsSamples.Customers
{
	public class CustomersManager
	{
		private CustomersMemoryCache _cache;
        private string _command;


		public CustomersManager(CustomersMemoryCache cache, string command)
		{
			this._cache = cache;
            this._command = command;
		}

		public IEnumerable<Customer> GetCustomers()
		{
			Console.WriteLine("Get Customers");

			var user = Thread.CurrentPrincipal.Identity.Name;
			var customers = _cache.Get(user);

			if (customers == null)
			{
				Console.WriteLine("From DB");
                string connectionString;
				using (var dbContext = new Northwind())
				{
					dbContext.Configuration.LazyLoadingEnabled = false;
					dbContext.Configuration.ProxyCreationEnabled = false;
                    customers = dbContext.Customers.ToList();
                    connectionString = dbContext.Database.Connection.ConnectionString;
                    
				}
                SqlDependency.Start(connectionString);
                _cache.Set(user, customers, DateTime.Now.AddDays(1));
            }

			return customers;
		}
        private CacheItemPolicy GetMonitor(string query, string connectionString)
        {
            var policy = new CacheItemPolicy();
            policy.ChangeMonitors.Add(
                new SqlChangeMonitor(
                    new SqlDependency(
                        new SqlCommand(query, new SqlConnection(connectionString)))));
            return policy;
        }
    }
}
