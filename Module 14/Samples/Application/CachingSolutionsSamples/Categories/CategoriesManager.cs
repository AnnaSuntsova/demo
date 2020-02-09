using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;


namespace CachingSolutionsSamples
{
	public class CategoriesManager
	{
		private CategoriesMemoryCache _cache;
        private string _command;

		public CategoriesManager(CategoriesMemoryCache cache, string command)
		{
			this._cache = cache;
            this._command = command;            
		}

		public IEnumerable<Category> GetCategories()
		{
			Console.WriteLine("Get Categories");

			var user = Thread.CurrentPrincipal.Identity.Name;
			var categories = _cache.Get(user);

			if (categories == null)
			{
				Console.WriteLine("From DB");
                string connectionString;
				using (var dbContext = new Northwind())
				{
					dbContext.Configuration.LazyLoadingEnabled = false;
					dbContext.Configuration.ProxyCreationEnabled = false;
					categories = dbContext.Categories.ToList();
                    connectionString = dbContext.Database.Connection.ConnectionString;                    
				}
                SqlDependency.Start(connectionString);
                _cache.Set(user, categories, GetMonitor(_command, connectionString));
            }
			return categories;
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
