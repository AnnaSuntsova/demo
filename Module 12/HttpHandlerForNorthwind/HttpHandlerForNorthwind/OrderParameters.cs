using System;
namespace HttpHandlerForNorthwind
{
    public class OrderParameters
    {
        public string CustomerId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}