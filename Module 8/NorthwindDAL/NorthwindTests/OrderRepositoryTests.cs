using NUnit.Framework;
using NorthwindDAL;
using System.Configuration;
using System.Data.Common;
using System.Collections.Generic;
using System;
using System.Data;
using resources = NorthwindTests.Resources;

namespace NorthwindTests
{
    public class OrderRepositoryTests
    {
        static void Main(string[] args)
        {
            
        }
        private OrderRepository repository;

        [SetUp]
        public void Setup()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["NorthwindConnection"];
            var providerName = connectionString.ProviderName;
            DbProviderFactories.GetFactory(providerName);        
            repository = new OrderRepository(connectionString);
        }

        [Test]
        public void GetOrdersTest()
        {
            var countExp = 0;
            using (var connection = repository.providerFactory.CreateConnection())
            {
                connection.ConnectionString = repository.connectString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = resources.GetOrdersTestQuery;
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
                ShippedDate = new DateTime(1996, 07, 16),
                OrderStatus = StatusState.Done,
                OrderDetails = details
            };

            Assert.That(orderExp, Is.EqualTo(orderReal).Using(new OrderComparer()));
        }
        
        [Test]
        public void AddNewTest()
        {
            var orderExp = new Order
            {
                OrderDate = null
            };
            
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
            orderExp.OrderDetails = details;

            var orderReal = repository.Add(orderExp);
            Assert.AreEqual(orderExp, orderReal);
        }

        [Test]
        public void DeleteDoneOrdersTest()
        {
            var ordId = 0;
            using (var connection = repository.providerFactory.CreateConnection())
            {
                connection.ConnectionString = repository.connectString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = resources.GetTop1New;
                    command.CommandType = CommandType.Text;
                    ordId = (int)command.ExecuteScalar();
                }
            }
            Assert.Throws<ArgumentOutOfRangeException>(() => repository.DeleteOrder(ordId));
        }

        [Test]
        public void DeleteInWorkOrdersTest()
        {
            var ordId = 0;
            using (var connection = repository.providerFactory.CreateConnection())
            {
                connection.ConnectionString = repository.connectString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = resources.GetTop1InWork;
                    command.CommandType = CommandType.Text;
                    ordId = (int)command.ExecuteScalar();
                }
            }
            var orderReal = repository.DeleteOrder(ordId);
            Assert.IsNull(orderReal);
        }

        [Test]
        public void UpdateOrderedDateTest()
        {
            const int orderId = 10317;

            var orderReal = repository.SetInWork(orderId);

            var details = new List<OrderInfo>();
            var chai = new OrderInfo()
            {
                ProductId = 1,
                ProductName = "Chai",
                Quantity = 20,
                UnitPrice = 14.40m,
                Discount = 0
            };
            details.Add(chai);

            var orderExp = new Order
            {
                OrderID = orderId,
                OrderStatus = StatusState.Done,
                OrderDate = DateTime.Now.Date,
                OrderDetails = details,
                ShippedDate = new DateTime(1996, 10, 10)
            };

            Assert.That(orderExp, Is.EqualTo(orderReal).Using(new OrderComparer()));
        }

        [Test]
        public void UpdateDoneOrderTest()
        {
            const int orderIdChange = 10268;

            var order = repository.GetOrderInfo(orderIdChange);
            order = repository.SetDone(order.OrderID);
            order = repository.UpdateOrder(order);
            Assert.IsNull(order);
        }

        [Test]
        public void UpdateNewOrderTest()
        {
            var ordIdExample = 0;
            using (var connection = repository.providerFactory.CreateConnection())
            {
                connection.ConnectionString = repository.connectString;
                connection.Open();
                
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = resources.GetTop1ShipIsNull;
                    command.CommandType = CommandType.Text;
                    ordIdExample = (int)command.ExecuteScalar();
                }
            }

            using (var connection = repository.providerFactory.CreateConnection())
            {
                connection.ConnectionString = repository.connectString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = resources.SetOrderDate;
                    command.CommandType = CommandType.Text;
                    var paramOrderId = command.CreateParameter();
                    paramOrderId.ParameterName = "@ordId";
                    paramOrderId.Value = ordIdExample;
                    command.Parameters.Add(paramOrderId);
                    var reader = command.ExecuteNonQuery();
                }
            }

            var order = repository.GetOrderInfo(ordIdExample);
            if (order.OrderDetails.Count > 0)
            {
                order.OrderDetails[0].ProductName = "Test";
            }
            else
            {
                var geitost = new OrderInfo()
                {
                    ProductId = 33,
                    ProductName = "Geitost",
                    Quantity = 24,
                    UnitPrice = 2.0000m
                };
                order.OrderDetails.Add(geitost);
            }
            order = repository.UpdateOrder(order);

            var orderReal = repository.GetOrderInfo(order.OrderID);

            Assert.That(order, Is.EqualTo(orderReal).Using(new OrderDetailsComparer()));
        }

        [Test]
        public void UpdateShippedDateTest()
        {
            const int orderId = 10271;

            var orderReal = repository.SetDone(orderId);

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
                ShippedDate = DateTime.Now.Date
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

    class OrderDetailsComparer : IEqualityComparer<Order>
    {
        public bool Equals(Order x, Order y)
        {
            var result = false;
            for (int i = 0; i < x.OrderDetails.Count; i++)
            {
                result = (x.OrderDetails[i].ProductId == y.OrderDetails[i].ProductId &&
                    x.OrderDetails[i].ProductName == y.OrderDetails[i].ProductName &&
                    x.OrderDetails[i].Quantity == y.OrderDetails[i].Quantity &&
                    x.OrderDetails[i].UnitPrice == y.OrderDetails[i].UnitPrice) &&
                    x.OrderDetails[i].Discount == y.OrderDetails[i].Discount ? true : false;
            }
            return result;
        }

        public int GetHashCode(Order obj)
        {
            return obj.GetHashCode();
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

    class OrderComparer : IEqualityComparer<Order>
    {
        public bool Equals(Order x, Order y)
        {
            var simpleObj = x.OrderDate == y.OrderDate
                && x.ShippedDate == y.ShippedDate
                && x.OrderID == y.OrderID
                && x.OrderStatus == y.OrderStatus ? true : false;

            var seqObj = false;
            for (int i = 0; i < x.OrderDetails.Count; i++)
            {
                seqObj = (x.OrderDetails[i].ProductId == y.OrderDetails[i].ProductId &&
                    x.OrderDetails[i].ProductName == y.OrderDetails[i].ProductName &&
                    x.OrderDetails[i].Quantity == y.OrderDetails[i].Quantity &&
                    x.OrderDetails[i].UnitPrice == y.OrderDetails[i].UnitPrice) &&
                    x.OrderDetails[i].Discount == y.OrderDetails[i].Discount ? true : false;
            }
            return (simpleObj && seqObj) ? true : false;
        }

        public int GetHashCode(Order obj)
        {
            return obj.GetHashCode();
        }
    }
}