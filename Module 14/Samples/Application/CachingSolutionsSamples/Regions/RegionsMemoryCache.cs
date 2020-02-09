using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindLibrary;
using System.Runtime.Caching;

namespace CachingSolutionsSamples.Regions
{
	public class RegionsMemoryCache : IRegionsCache
    {
		ObjectCache cache = MemoryCache.Default;
		string prefix  = "Cache_Regions";

		public IEnumerable<Region> Get(string forUser)
		{
			return (IEnumerable<Region>) cache.Get(prefix + forUser);
		}

		public void Set(string forUser, IEnumerable<Region> regions, DateTimeOffset expirationDate)
		{
			cache.Set(prefix + forUser, regions, ObjectCache.InfiniteAbsoluteExpiration);
		}
	}
}
