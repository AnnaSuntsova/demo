using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindLibrary;
using System.Runtime.Caching;

namespace CachingSolutionsSamples
{
	public class CategoriesMemoryCache : ICategoriesCache
	{
		ObjectCache cache = MemoryCache.Default;
		string prefix  = "Cache_Categories";

		public IEnumerable<Category> Get(string forUser)
		{
			return (IEnumerable<Category>) cache.Get(prefix + forUser);
		}

		public void Set(string forUser, IEnumerable<Category> categories, DateTimeOffset expirationDate)
		{
			cache.Set(prefix + forUser, categories, expirationDate);
		}

        public void Set(string key, IEnumerable<Category> value, CacheItemPolicy policy)
        {
            cache.Set(prefix + key, value, policy);
        }

    }
}
