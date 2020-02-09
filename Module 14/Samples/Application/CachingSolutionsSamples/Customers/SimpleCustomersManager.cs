using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Customers
{
    public class SimpleCustomersManager
    {
        private ICustomersCache _cache;


        public SimpleCustomersManager(ICustomersCache cache)
        {
            this._cache = cache;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            Console.WriteLine("Get Customers");

            var user = Thread.CurrentPrincipal.Identity.Name;
            var customers = _cache.Get(user);

            if (customers == null)
            {
                Console.WriteLine("From DB");
                using (var dbContext = new Northwind())
                {
                    dbContext.Configuration.LazyLoadingEnabled = false;
                    dbContext.Configuration.ProxyCreationEnabled = false;
                    customers = dbContext.Customers.ToList();
                }
                _cache.Set(user, customers, DateTime.Now.AddDays(1));
            }

            return customers;
        }
    }
}
