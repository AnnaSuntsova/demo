using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindLibrary;
using System.Runtime.Caching;

namespace CachingSolutionsSamples.Customers
{
	public class CustomersMemoryCache : ICustomersCache
    {
		ObjectCache cache = MemoryCache.Default;
		string prefix  = "Cache_Customers";

		public IEnumerable<Customer> Get(string forUser)
		{
			return (IEnumerable<Customer>) cache.Get(prefix + forUser);
		}

		public void Set(string forUser, IEnumerable<Customer> customers, DateTimeOffset expirationDate)
		{
			cache.Set(prefix + forUser, customers, ObjectCache.InfiniteAbsoluteExpiration);
		}

        public void Set(string key, IEnumerable<Category> value, CacheItemPolicy policy)
        {
            cache.Set(prefix + key, value, policy);
        }
    }
}
