using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Categories
{
    public class SimpleCategoriesManager
    {
        private readonly ICategoriesCache _cache;

        public SimpleCategoriesManager(ICategoriesCache cache)
        {
            _cache = cache;
        }

        public IEnumerable<Category> GetCategories()
        {
            Console.WriteLine("Get Categories");

            var user = Thread.CurrentPrincipal.Identity.Name;
            var categories = _cache.Get(user);

            if (categories == null)
            {
                Console.WriteLine("From DB");
                using (var dbContext = new Northwind())
                {
                    dbContext.Configuration.LazyLoadingEnabled = false;
                    dbContext.Configuration.ProxyCreationEnabled = false;
                    categories = dbContext.Categories.ToList();                    
                }
                _cache.Set(user, categories, DateTimeOffset.Now.AddDays(1));
            }
            return categories;
        }
    }
}
