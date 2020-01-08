namespace NorthwindDAL
{
    public class OrderDetails
    {
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public int Discount { get; set; }
        public decimal ExtendedPrice { get; set; }
    }
}