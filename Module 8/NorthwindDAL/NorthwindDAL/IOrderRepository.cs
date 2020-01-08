using System;
using System.Collections.Generic;

namespace NorthwindDAL
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders();
        Order GetOrderInfo(int id);
        Order Add(Order newOrder);
        int DeleteOrders();
        Order UpdateOrderedDate(int id, DateTime ordDate);
        Order UpdateShippedDate(int id, DateTime shipDate);
        Order UpdateOrder(int id, Order order);
        OrderHist GetOrderHists(string id);
        CustOrdersDetails GetOrderDetails(int id);
    }
}
