using NUnit.Framework;
using NorthwindDAL;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
using System.Data;

namespace NorthwindTests
{
    public class OrderRepositoryTests
    {
        private OrderRepository repository; 
        [SetUp]
        public void Setup()
        {
            DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);
            //var connectionStringItem = ConfigurationManager.ConnectionStrings["NorthwindConnection"];
            var connectionString = @"Data Source = LAPTOP-BCCB028D\SQLEXPRESS; Initial Catalog = Northwind; Integrated Security = True"; /*connectionStringItem.ConnectionString;*/
            var providerName = "System.Data.SqlClient" /*connectionStringItem.ProviderName*/;
            repository = new OrderRepository(connectionString, providerName);
        }

        [Test]
        public void GetOrdersTest()
        {
            var countExp = 0;
            using (var connection = repository.providerFactory.CreateConnection())
            {
                connection.ConnectionString = repository.connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select count(OrderID) as rowsCount from Orders ";
                    command.CommandType = CommandType.Text;
                    countExp = (int)command.ExecuteScalar();                    
                }
            }
            var orders = repository.GetOrders();
            var countReal = 0;
            using (IEnumerator<Order> enumerator = orders.GetEnumerator())
            {
                while (enumerator.MoveNext())
                    countReal++;
            }
            Assert.AreEqual(countExp, countReal);
        }        

        [Test]
        public void GetOrderInfoTest()
        {
            const int id = 10248;
            var orderReal = repository.GetOrderInfo(id);
            var details = new List<OrderInfo>();
            var quesoCabrales = new OrderInfo
            {
                ProductId = 11,
                ProductName = "Queso Cabrales",
                Quantity = 12,
                UnitPrice = 14.0000m
            };
            details.Add(quesoCabrales);
            var singaporean = new OrderInfo
            {
                ProductId = 42,
                ProductName = "Singaporean Hokkien Fried Mee",
                Quantity = 10,
                UnitPrice = 9.8000m
            };
            details.Add(singaporean);
            var mozzarella = new OrderInfo
            {
                ProductId = 72,
                ProductName = "Mozzarella di Giovanni",
                Quantity = 5,
                UnitPrice = 34.8000m
            };
            details.Add(mozzarella);

            var orderExp = new Order
            {
                OrderID = 10248,
                OrderDate = new DateTime(1996, 07, 04), 
                ShippedDate = new DateTime (1996, 07, 16),
                OrderStatus = StatusState.Done,
                OrderDetails = details
            };

            Assert.That(orderExp, Is.EqualTo(orderReal).Using(new OrderComparer()));
        }

        [Test]
        public void AddInWorkTest()
        {
            var orderExp = new Order
            {
                OrderDate = new DateTime(2019, 07, 01),
                OrderStatus = StatusState.InWork
            };
            var orderReal = repository.Add(orderExp);
            Assert.AreEqual(orderExp, orderReal);
        }

        [Test]
        public void AddNewTest()
        {
            var orderExp = new Order
            {
                OrderDate = null,
                OrderStatus = StatusState.New
            };
            var orderReal = repository.Add(orderExp);
            Assert.AreEqual(orderExp, orderReal);
        }

        //[Test]
        //public void DeleteOrdersTest()
        //{
        //    var countExp = 0;
        //    using (var connection = repository.providerFactory.CreateConnection())
        //    {
        //        connection.ConnectionString = repository.connectionString;
        //        connection.Open();

        //        using (var command = connection.CreateCommand())
        //        {
        //            command.CommandText = "select count(OrderID) as rowsCount from Orders where OrderDate is null or (OrderDate is not null and ShippedDate is null) ";
        //            command.CommandType = CommandType.Text;
        //            countExp = (int)command.ExecuteScalar();
        //        }
        //    }
        //    var countReal = repository.DeleteOrders();
        //    Assert.AreEqual(countExp, countReal);
        //}
        [Test]
        public void UpdateOrderedDateTest()
        {
            const int orderId = 10317;
            DateTime ordDate = new DateTime(2019, 12, 12);

            var orderReal = repository.UpdateOrderedDate(orderId, ordDate);

            var details = new List <OrderInfo>();
            var chai = new OrderInfo()
            {
                ProductId = 1,
                ProductName = "Chai",
                Quantity = 20,
                UnitPrice = 14.40m
            };
            details.Add(chai);

            var orderExp = new Order
            {
                OrderID = orderId,
                OrderStatus = StatusState.Done,
                OrderDate = ordDate,
                OrderDetails = details,
                ShippedDate = new DateTime(1996, 10, 10)
            };

            Assert.That(orderExp, Is.EqualTo(orderReal).Using(new OrderComparer()));
        }

        [Test]
        public void UpdateDoneOrderTest()
        {
            const int orderIdChange = 10268;
            const int orderIdOrig = 10261;

            var order = repository.GetOrderInfo(orderIdOrig);
            var orderReal = repository.UpdateOrder(orderIdChange, order);
            Assert.IsNull(orderReal);
        }

        [Test]
        public void UpdateNewOrderTest()
        {
            const int orderIdChange = 11125;
            const int orderIdOrig = 10261;

            var order = repository.GetOrderInfo(orderIdOrig);
            var orderReal = repository.UpdateOrder(orderIdChange, order);

            Assert.That(order, Is.EqualTo(null).Using(new OrderComparer()));
        }

        [Test]
        public void UpdateShippedDateTest()
        {
            const int orderId = 10271;
            DateTime shipDate = new DateTime(2020, 07, 12);

            var orderReal = repository.UpdateShippedDate(orderId, shipDate);

            var details = new List<OrderInfo>();
            var geitost = new OrderInfo()
            {
                ProductId = 33,
                ProductName = "Geitost",
                Quantity = 24,
                UnitPrice = 2.0000m
            };
            details.Add(geitost);

            var orderExp = new Order
            {
                OrderID = orderId,
                OrderStatus = StatusState.Done,
                OrderDate = new DateTime(1996, 08, 01),
                OrderDetails = details,
                ShippedDate = shipDate
            };

            Assert.That(orderExp, Is.EqualTo(orderReal).Using(new OrderComparer()));
        }

        [Test]
        public void GetOrderHistsTest()
        {
            const string custId = "LAZYK";

            var histReal = repository.GetOrderHists(custId);

            var histExp = new OrderHist
            {
                CustHist = new List<CustOrderHist>()
            };
            var prodBoston = new CustOrderHist()
            {
                ProductName = "Boston Crab Meat",
                Total = 10
            };
            histExp.CustHist.Add(prodBoston);

            var prodQueso = new CustOrderHist()
            {
                ProductName = "Queso Cabrales",
                Total = 10
            };
            histExp.CustHist.Add(prodQueso);

            Assert.That(histExp, Is.EqualTo(histReal).Using(new OrderHistComparer()));
        }

        [Test]
        public void GetOrderDetailsTest()
        {
            const int orderId = 10249;

            var orderDetailsReal = repository.GetOrderDetails(orderId);
                
            var orderDetailsExp = new CustOrdersDetails
            {
                CustDetails = new List<OrderDetails>()
            };
            var prodTofu = new OrderDetails()
            {
                ProductName = "Tofu",
                UnitPrice = 18.60m,
                Quantity = 9,
                Discount = 0,
                ExtendedPrice = 167.40m
            };
            orderDetailsExp.CustDetails.Add(prodTofu);

            var prodManjimup = new OrderDetails()
            {
                ProductName = "Manjimup Dried Apples",
                UnitPrice = 42.40m,
                Quantity = 40,
                Discount = 0,
                ExtendedPrice = 1696.00m
            };
            orderDetailsExp.CustDetails.Add(prodManjimup);

            Assert.That(orderDetailsExp, Is.EqualTo(orderDetailsReal).Using(new OrderDetailComparer()));
        }
    }

    class OrderDetailComparer : IEqualityComparer<CustOrdersDetails>
    {
        public bool Equals(CustOrdersDetails x, CustOrdersDetails y)
        {
            var result = false;
            for (int i = 0; i < x.CustDetails.Count; i++)
            {
                result = (x.CustDetails[i].ProductName == y.CustDetails[i].ProductName &&
                          x.CustDetails[i].Quantity == y.CustDetails[i].Quantity &&
                          x.CustDetails[i].Discount == y.CustDetails[i].Discount &&
                          x.CustDetails[i].ExtendedPrice == y.CustDetails[i].ExtendedPrice) ? true : false;
            }
            return result;
        }

        public int GetHashCode(CustOrdersDetails obj)
        {
            return obj.GetHashCode();
        }
    }

    class OrderHistComparer : IEqualityComparer<OrderHist>
    {
        public bool Equals(OrderHist x, OrderHist y)
        {
            var result = false;
            for (int i = 0; i < x.CustHist.Count; i++)
            {
                result = (x.CustHist[i].ProductName == y.CustHist[i].ProductName &&
                    x.CustHist[i].Total == y.CustHist[i].Total) ? true : false;
            }
            return result;
        }

        public int GetHashCode(OrderHist obj)
        {
            return obj.GetHashCode();
        }
    }

    class OrderComparer: IEqualityComparer<Order>
    {
        public bool Equals(Order x, Order y)
        {
            var simpleObj = x.OrderDate == y.OrderDate
                && x.ShippedDate == y.ShippedDate
                && x.OrderID == y.OrderID
                && x.OrderStatus == y.OrderStatus ? true : false;

            var seqObj = false;
            for (int i=0; i<x.OrderDetails.Count; i++)
            {
                seqObj = (x.OrderDetails[i].ProductId == y.OrderDetails[i].ProductId &&
                    x.OrderDetails[i].ProductName == y.OrderDetails[i].ProductName &&
                    x.OrderDetails[i].Quantity == y.OrderDetails[i].Quantity &&
                    x.OrderDetails[i].UnitPrice == y.OrderDetails[i].UnitPrice) ? true : false;                
            }
            return (simpleObj && seqObj) ? true : false;
        }

        public int GetHashCode(Order obj)
        {
           return obj.GetHashCode();
        }
    }
}