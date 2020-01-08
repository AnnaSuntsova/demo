using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;

namespace NorthwindDAL
{
    public class OrderRepository : IOrderRepository
    {
        public readonly DbProviderFactory providerFactory;
        public readonly string connectionString;

        public OrderRepository (string conString, string provider)
        {
            providerFactory = DbProviderFactories.GetFactory(provider);
            connectionString = conString;
        }

        public Order Add(Order newOrder)
        {
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into dbo.Orders (OrderDate, ShippedDate) "+
                                          "values (@ordDate, @shipDate); "+
                                          "SET @newId = SCOPE_IDENTITY()";
                    command.CommandType = CommandType.Text;

                    var paramOrdDate = command.CreateParameter();
                    paramOrdDate.ParameterName = "@ordDate";

                    var paramShipDate = command.CreateParameter();
                    paramShipDate.ParameterName = "@shipDate";                    

                    if (newOrder.OrderStatus == StatusState.InWork)
                    {
                        paramOrdDate.Value = newOrder.OrderDate;
                        paramShipDate.Value = DBNull.Value;
                    }
                    else if (newOrder.OrderStatus == StatusState.New)
                    {
                        paramOrdDate.Value = DBNull.Value;
                        paramShipDate.Value = DBNull.Value;
                    }
                    else
                    {
                        paramOrdDate.Value = newOrder.OrderDate;
                        paramShipDate.Value = newOrder.ShippedDate;
                    }

                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@newId";
                    paramId.Direction = ParameterDirection.Output;
                    paramId.Size = 50;

                    command.Parameters.Add(paramOrdDate);
                    command.Parameters.Add(paramShipDate);
                    command.Parameters.Add(paramId);

                    var nonQuery = command.ExecuteNonQuery();

                    var orderNew = GetOrderInfo(int.Parse(paramId.Value.ToString()));
                    return newOrder;
                }
            }            
        }

        public int DeleteOrders()
        {
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from [Order Details] where OrderId" +
                        "delete from Orders where OrderDate is null or (OrderDate is not null and ShippedDate is null) ";
                    command.CommandType = CommandType.Text;
                    var numDelRows = command.ExecuteNonQuery();
                    return numDelRows;
                }
            }
        }

        public CustOrdersDetails GetOrderDetails(int id)
        {
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CustOrdersDetail";
                    command.CommandType = CommandType.StoredProcedure;
                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@OrderID";
                    paramId.Value = id;
                    command.Parameters.Add(paramId);

                    using (var reader = command.ExecuteReader())
                    {
                        var orderDetails = new CustOrdersDetails
                        {
                            CustDetails = new List<OrderDetails>()
                        };
                        if (!reader.HasRows)
                            return null;
                        while (reader.Read())
                        {
                            var order = new OrderDetails
                            {
                                ProductName = (string)reader["ProductName"],
                                UnitPrice = (decimal)reader["UnitPrice"], 
                                Quantity = reader.GetInt16(2),
                                Discount = (int)reader["Discount"], 
                                ExtendedPrice = (decimal)reader["ExtendedPrice"]
                            };
                            orderDetails.CustDetails.Add(order);
                        }
                        return orderDetails;
                    }
                }
            }
        }

        public OrderHist GetOrderHists(string id)
        {
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CustOrderHist";
                    command.CommandType = CommandType.StoredProcedure;
                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@CustomerID";
                    paramId.Value = id;
                    command.Parameters.Add(paramId);

                    using (var reader = command.ExecuteReader())
                    {
                        var orderHist = new OrderHist
                        {
                            CustHist = new List<CustOrderHist>()
                        };
                        if (!reader.HasRows)
                            return null;
                        while (reader.Read())
                        {
                            var order = new CustOrderHist
                            {
                                ProductName = (string)reader["ProductName"],
                                Total = (int)reader["Total"]
                            };
                            orderHist.CustHist.Add(order);
                        }
                        return orderHist;
                    }
                }
            }
        }

        public Order GetOrderInfo(int id)
        {
            using (var connection = providerFactory.CreateConnection())
            {                
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select OrderID, OrderDate, ShippedDate from dbo.Orders where OrderId = @id; " +
                                          "select OrderId, Quantity, t1.UnitPrice, ProductName, t1.ProductId from dbo.[Order Details] t1 " +
                                          "left outer join dbo.[Products] t2 on t1.ProductID = t2.ProductID "+
                                          "where OrderId = @id";
                    command.CommandType = CommandType.Text;
                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = id;
                    command.Parameters.Add(paramId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                            return null;

                        Order order = new Order();
                        order.OrderDetails = new List<OrderInfo>();
                        while (reader.Read())
                        {
                            order.OrderID = reader.GetInt32(0);
                            order.OrderDate = (reader.IsDBNull(1)) ? (DateTime?)null : reader.GetDateTime(1);
                            order.ShippedDate = (reader.IsDBNull(2)) ? (DateTime?)null : reader.GetDateTime(2);
                            order.OrderStatus = (reader.IsDBNull(1)) ? StatusState.New :
                                                (reader.IsDBNull(2)) ? StatusState.InWork : StatusState.Done;
                        }
                        reader.NextResult();

                        while (reader.Read())
                        {
                            var detail = new OrderInfo()
                            {
                                Quantity = reader.GetInt16(1),
                                UnitPrice = reader.GetDecimal(2),
                                ProductName = reader.GetString(3),
                                ProductId = reader.GetInt32(4)
                            };
                            order.OrderDetails.Add(detail);
                        }
                        return order;
                    }
                }
            }           
        }

        public IEnumerable<Order> GetOrders()
        {
            var resultOrders = new List<Order>();
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select OrderID, OrderDate, ShippedDate from dbo.Orders";
                    command.CommandType = CommandType.Text;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var order = new Order
                            {
                                OrderID = reader.GetInt32(0),
                                OrderDate = reader.GetDateTime(1),
                                ShippedDate = (reader.IsDBNull(2)) ? DateTime.MinValue : reader.GetDateTime(2),
                                OrderStatus = (reader.IsDBNull(1)) ? StatusState.New : 
                                              (reader.IsDBNull(2)) ? StatusState.InWork : StatusState.Done
                            };
                            resultOrders.Add(order);
                        }
                    }
                }
            }
            return resultOrders;
        }

        public Order UpdateOrder(int id, Order order)
        {
            var existOrder = GetOrderInfo(id);
            if (existOrder.OrderStatus == StatusState.InWork || existOrder.OrderStatus == StatusState.Done)
                return null;
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                
                    foreach (var item in order.OrderDetails)
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = "update dbo.[Order Details] set UnitPrice = @unitPrice, Quantity = @quantity where OrderId = @id and ProductId = @prodId";
                            command.CommandType = CommandType.Text;

                            var paramUnitPrice = command.CreateParameter();
                            paramUnitPrice.ParameterName = "@unitPrice";
                            paramUnitPrice.Value = item.UnitPrice;

                            var paramDiscount = command.CreateParameter();
                            paramDiscount.ParameterName = "@quantity";
                            paramDiscount.Value = item.Quantity;

                            var paramOrdId = command.CreateParameter();
                            paramOrdId.ParameterName = "@id";
                            paramOrdId.Value = id;

                            var paramProdId = command.CreateParameter();
                            paramProdId.ParameterName = "@prodId";
                            paramProdId.Value = item.ProductId;

                            command.Parameters.Add(paramUnitPrice);
                            command.Parameters.Add(paramDiscount);
                            command.Parameters.Add(paramOrdId);
                            command.Parameters.Add(paramProdId);

                            var reader = command.ExecuteNonQuery();
                        }
                    }
                var resOrder = GetOrderInfo(id);
                return resOrder;
            }
        }

        public Order UpdateOrderedDate(int id, DateTime ordDate)
        {
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "update dbo.Orders set OrderDate = @ordDate where OrderID = @id";                    
                    command.CommandType = CommandType.Text;

                    var paramOrdDate = command.CreateParameter();
                    paramOrdDate.ParameterName = "@ordDate";
                    paramOrdDate.Value = ordDate;

                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = id;

                    command.Parameters.Add(paramOrdDate);
                    command.Parameters.Add(paramId);

                    var reader = command.ExecuteNonQuery();
                }               
            }
            var resOrder = GetOrderInfo(id);
            return resOrder;
        }

        public Order UpdateShippedDate(int id, DateTime shipDate)
        {
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "update dbo.Orders set ShippedDate = @shipDate where OrderID = @id";
                    command.CommandType = CommandType.Text;

                    var paramShipDate = command.CreateParameter();
                    paramShipDate.ParameterName = "@shipDate";
                    paramShipDate.Value = shipDate;

                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = id;

                    command.Parameters.Add(paramShipDate);
                    command.Parameters.Add(paramId);

                    var reader = command.ExecuteNonQuery();
                }
                var resOrder = GetOrderInfo(id);
                return resOrder;
            }
        }
    }
}
