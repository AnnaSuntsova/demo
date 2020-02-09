using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Regions
{
    public class SimpleRegionsManager
    {
        private IRegionsCache _cache;

        public SimpleRegionsManager(IRegionsCache cache)
        {
            this._cache = cache;
        }

        public IEnumerable<Region> GetRegions()
        {
            Console.WriteLine("Get Regions");

            var user = Thread.CurrentPrincipal.Identity.Name;
            var regions = _cache.Get(user);

            if (regions == null)
            {
                Console.WriteLine("From DB");
                using (var dbContext = new Northwind())
                {
                    dbContext.Configuration.LazyLoadingEnabled = false;
                    dbContext.Configuration.ProxyCreationEnabled = false;
                    regions = dbContext.Regions.ToList();
                }
                _cache.Set(user, regions, DateTime.Now.AddDays(1));
            }

            return regions;
        }
    }
}
