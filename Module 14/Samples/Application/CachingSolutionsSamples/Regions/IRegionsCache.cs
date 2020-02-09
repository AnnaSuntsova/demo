using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Regions
{
	public interface IRegionsCache
    {
		IEnumerable<Region> Get(string forUser);
		void Set(string forUser, IEnumerable<Region> regions, DateTimeOffset expirationDate);
	}
}
