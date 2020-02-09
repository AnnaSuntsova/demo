using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Customers
{
	public interface ICustomersCache
    {
		IEnumerable<Customer> Get(string forUser);
		void Set(string forUser, IEnumerable<Customer> customers, DateTimeOffset expirationDate);
	}
}
