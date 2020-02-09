using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Regions
{
	public class RegionsManager
	{
		private RegionsMemoryCache _cache;
        private string _command;

		public RegionsManager(RegionsMemoryCache cache, string command)
		{
			_cache = cache;
            _command = command;
		}

		public IEnumerable<Region> GetRegions()
		{
			Console.WriteLine("Get Regions");

			var user = Thread.CurrentPrincipal.Identity.Name;
			var regions = _cache.Get(user);

			if (regions == null)
			{
				Console.WriteLine("From DB");
                string connectionString;
				using (var dbContext = new Northwind())
				{
					dbContext.Configuration.LazyLoadingEnabled = false;
					dbContext.Configuration.ProxyCreationEnabled = false;
                    regions = dbContext.Regions.ToList();
                    connectionString = dbContext.Database.Connection.ConnectionString;
                }
                SqlDependency.Start(connectionString);
                _cache.Set(user, regions, DateTime.Now.AddDays(1));
            }
            
			return regions;
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
