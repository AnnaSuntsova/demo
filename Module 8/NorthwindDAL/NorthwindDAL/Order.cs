using System;
using System.Collections.Generic;

namespace NorthwindDAL
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public StatusState OrderStatus { get; set; }
        public List<OrderInfo> OrderDetails { get; set; }
    }
}