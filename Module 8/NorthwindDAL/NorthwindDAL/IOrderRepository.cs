using System;
using System.Collections.Generic;

namespace NorthwindDAL
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetOrders();
        Order GetOrderInfo(int id);
        Order Add(Order newOrder);
        Order DeleteOrder(int id);
        Order SetInWork(int id);
        Order SetDone(int id);
        Order UpdateOrder(Order order);
        OrderHist GetOrderHists(string id);
        CustOrdersDetails GetOrderDetails(int id);
    }
}
