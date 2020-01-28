using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Configuration;
using resources = NorthwindDAL.Resources;

namespace NorthwindDAL
{
    public class OrderRepository : IOrderRepository
    {
        public readonly DbProviderFactory providerFactory;
        public readonly string connectString;

        public OrderRepository (ConnectionStringSettings connectionString)
        {
            providerFactory = DbProviderFactories.GetFactory(connectionString.ProviderName);
            connectString = connectionString.ConnectionString;
        }

        public Order Add(Order newOrder)
        {
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectString;
                connection.Open();

                DbParameter paramId;
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = resources.InsertOrderGetId;
                    command.CommandType = CommandType.Text;

                    var paramOrdDate = command.CreateParameter();
                    paramOrdDate.ParameterName = "@ordDate";

                    var paramShipDate = command.CreateParameter();
                    paramShipDate.ParameterName = "@shipDate";
                    paramOrdDate.Value = DBNull.Value;
                    paramShipDate.Value = DBNull.Value;

                    paramId = command.CreateParameter();
                    paramId.ParameterName = "@newId";
                    paramId.Direction = ParameterDirection.Output;
                    paramId.Size = 50;

                    command.Parameters.Add(paramOrdDate);
                    command.Parameters.Add(paramShipDate);
                    command.Parameters.Add(paramId);

                    var nonQuery = command.ExecuteNonQuery();
                }

                InsertDetails(newOrder, connection, int.Parse(paramId.Value.ToString()));

                var orderNew = GetOrderInfo(int.Parse(paramId.Value.ToString()));
                return newOrder;
            }
        }       

        public Order DeleteOrder(int id)
        {
            var order = GetOrderInfo(id);
            if (order.OrderStatus == StatusState.Done)
                throw new ArgumentOutOfRangeException();

            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = resources.DeleteOrderInfo;
                    command.CommandType = CommandType.Text;
                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@OrderID";
                    paramId.Value = id;
                    command.Parameters.Add(paramId);

                    command.ExecuteNonQuery();
                }
            }
            var resOrder = GetOrderInfo(id);
            return resOrder;
        }

        public CustOrdersDetails GetOrderDetails(int id)
        {
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectString;
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
                connection.ConnectionString = connectString;
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
                connection.ConnectionString = connectString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = resources.GetOrderInfoQuery;
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
                                ProductId = reader.GetInt32(4),
                                Discount = reader.GetFloat(5)
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
                connection.ConnectionString = connectString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = resources.SimpleSelectFromOrders;
                    command.CommandType = CommandType.Text;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var order = new Order
                            {
                                OrderID = reader.GetInt32(0),
                                OrderDate = (reader.IsDBNull(1)) ?  (DateTime?) null : reader.GetDateTime(1),
                                ShippedDate = (reader.IsDBNull(2)) ? (DateTime?)null : reader.GetDateTime(2),
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

        public Order UpdateOrder(Order order)
        {           
            if (order.OrderStatus == StatusState.InWork || order.OrderStatus == StatusState.Done)
                return null;
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectString;
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = resources.DeleteOrderInfo;
                    command.CommandType = CommandType.Text;

                    var paramOrdId = command.CreateParameter();
                    paramOrdId.ParameterName = "@OrderID";
                    paramOrdId.Value = order.OrderID;

                    command.Parameters.Add(paramOrdId);

                    var reader = command.ExecuteNonQuery();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = resources.InsertOrderWithTheSameId;
                    command.CommandType = CommandType.Text;

                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@ordId";
                    paramId.Value = order.OrderID;

                    var paramOrdDate = command.CreateParameter();
                    paramOrdDate.ParameterName = "@ordDate";
                    if (order.OrderDate == null)
                        paramOrdDate.Value = DBNull.Value;
                    else paramOrdDate.Value = order.OrderDate;

                    var paramShipDate = command.CreateParameter();
                    paramShipDate.ParameterName = "@shipDate";
                    if (order.ShippedDate == null)
                        paramShipDate.Value = DBNull.Value;
                    else paramShipDate.Value = order.ShippedDate;

                    command.Parameters.Add(paramId);
                    command.Parameters.Add(paramOrdDate);
                    command.Parameters.Add(paramShipDate);

                    var reader = command.ExecuteNonQuery();
                }
                InsertDetails(order, connection, order.OrderID);

                var resOrder = GetOrderInfo(order.OrderID);
                return resOrder;
            }
        }

        public Order SetInWork(int id)
        {
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = resources.UpdateOrderDate;                    
                    command.CommandType = CommandType.Text;

                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = id;

                    command.Parameters.Add(paramId);

                    var reader = command.ExecuteNonQuery();
                }               
            }
            var resOrder = GetOrderInfo(id);
            return resOrder;
        }

        public Order SetDone(int id)
        {
            using (var connection = providerFactory.CreateConnection())
            {
                connection.ConnectionString = connectString;
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = resources.UpdateShippedDate;
                    command.CommandType = CommandType.Text;

                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = id;

                    command.Parameters.Add(paramId);

                    var reader = command.ExecuteNonQuery();
                }
                var resOrder = GetOrderInfo(id);
                return resOrder;
            }
        }

        private static void InsertDetails(Order order, DbConnection connection, int orderId)
        {
            foreach (var item in order.OrderDetails)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = resources.InsertIntoOrderDetails;
                    command.CommandType = CommandType.Text;

                    var paramUnitPrice = command.CreateParameter();
                    paramUnitPrice.ParameterName = "@unitPrice";
                    paramUnitPrice.Value = item.UnitPrice;

                    var paramQuant = command.CreateParameter();
                    paramQuant.ParameterName = "@quantity";
                    paramQuant.Value = item.Quantity;

                    var paramOrdId = command.CreateParameter();
                    paramOrdId.ParameterName = "@ordId";
                    paramOrdId.Value = orderId;

                    var paramProdId = command.CreateParameter();
                    paramProdId.ParameterName = "@prodId";
                    paramProdId.Value = item.ProductId;

                    var paramDiscount = command.CreateParameter();
                    paramDiscount.ParameterName = "@discount";
                    paramDiscount.Value = item.Discount;

                    command.Parameters.Add(paramUnitPrice);
                    command.Parameters.Add(paramQuant);
                    command.Parameters.Add(paramOrdId);
                    command.Parameters.Add(paramProdId);
                    command.Parameters.Add(paramDiscount);

                    var reader = command.ExecuteNonQuery();
                }
            }
        }
    }
}
