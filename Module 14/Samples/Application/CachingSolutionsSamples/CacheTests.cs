using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.Customers;
using CachingSolutionsSamples.Regions;
using CachingSolutionsSamples.Categories;

namespace CachingSolutionsSamples
{
	[TestClass]
	public class CacheTests
	{
		[TestMethod]
		public void CategoryMemoryCache()
		{
			var categoryManager = new SimpleCategoriesManager(new CategoriesMemoryCache());
            var categCnt = 8;
			for (var i = 0; i < 10; i++)
			{
                Assert.AreEqual(categCnt, categoryManager.GetCategories().Count());
				Thread.Sleep(100);
			}
		}

		[TestMethod]
		public void CategoryRedisCache()
		{
			var categoryManager = new SimpleCategoriesManager(new CategoriesRedisCache("localhost"));
            var categCnt = 8;
            for (var i = 0; i < 10; i++)
			{
                Assert.AreEqual(categCnt, categoryManager.GetCategories().Count());
                Thread.Sleep(100);
			}
		}

        [TestMethod]
        public void CustomersMemoryCache()
        {
            var customerManager = new SimpleCustomersManager(new CustomersMemoryCache());
            var custCnt = 91;
            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(custCnt, customerManager.GetCustomers().Count());
                Thread.Sleep(100);
            }
        }

        [TestMethod]
        public void CustomersRedisCache()
        {
            var customerManager = new SimpleCustomersManager(new CustomersRedisCache("localhost"));
            var custCnt = 91;
            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(custCnt, customerManager.GetCustomers().Count());
                Thread.Sleep(100);
            }
        }

        [TestMethod]
        public void RegionsMemoryCache()
        {
            var regionManager = new SimpleRegionsManager(new RegionsMemoryCache());
            var regCnt = 4;
            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(regCnt, regionManager.GetRegions().Count());
                Thread.Sleep(100);
            }
        }

        [TestMethod]
        public void RegionsRedisCache()
        {
            var regionManager = new SimpleRegionsManager(new RegionsRedisCache("localhost"));
            var regCnt = 4;
            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(regCnt, regionManager.GetRegions().Count());
                Thread.Sleep(100);
            }
        }

        [TestMethod]
        public void CategoryPolicyMemoryCache()
        {
            var query = "select * from category";
            var categoryManager = new CategoriesManager(new CategoriesMemoryCache(), query);
            var categCnt = 8;
            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(categCnt, categoryManager.GetCategories().Count());
                Thread.Sleep(100);
            }
        }

        [TestMethod]
        public void CustomersPolicyMemoryCache()
        {
            var query = "select * from customers";
            var customerManager = new CustomersManager(new CustomersMemoryCache(), query);
            var categCnt = 91;
            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(categCnt, customerManager.GetCustomers().Count());
                Thread.Sleep(100);
            }
        }

        [TestMethod]
        public void RegionsPolicyMemoryCache()
        {
            var query = "select * from category";
            var regionManager = new RegionsManager(new RegionsMemoryCache(), query);
            var categCnt = 4;
            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(categCnt, regionManager.GetRegions().Count());
                Thread.Sleep(100);
            }
        }
    }
}
