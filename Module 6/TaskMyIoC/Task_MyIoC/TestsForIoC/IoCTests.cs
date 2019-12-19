using NUnit.Framework;
using MyIoC;
using System.Reflection;

namespace Tests
{
    public class IoCTests
    {
        [Test]
        public void CreateInstanceWithoutDependencies()
        {
            var container = new Container();
            container.AddType(typeof(CustomerBLL));
            container.AddType(typeof(Logger));
            container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

            var customerBll = (CustomerBLL)container.CreateInstance(typeof(CustomerBLL));

            Assert.AreNotEqual(null, customerBll);
        }

        [Test]
        public void CreateInstanceWithDependencies()
        {
            var container = new Container();
            container.AddType(typeof(CustomerBLL));
            container.AddType(typeof(Logger));
            container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

            var customerBll = container.CreateInstance<CustomerBLL>();

            Assert.AreNotEqual(null, customerBll);
        }

        [Test]
        public void GetAssemblyAndCheckInstance ()
        {
            var container = new Container();
            container.AddAssembly(Assembly.Load("MyIoC"));

            var customerBll = container.CreateInstance<CustomerBLL>();

            Assert.AreNotEqual(null, customerBll);
        }
    }
}