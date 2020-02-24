using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace HttpHandlerForNorthwind
{
    [Serializable]
    [XmlRoot(ElementName = "catalog")]
    public class OrderFields
    {
        [XmlElement(ElementName = "customer")]
        public string Customer { get; set; }

        [XmlElement(ElementName = "orderDate")]
        public DateTime? OrderDate { get; set; }

        [XmlElement(ElementName = "freight")]
        public decimal? Freight { get; set; }

        [XmlElement(ElementName = "shipName")]
        public string ShipName { get; set; }

        [XmlElement(ElementName = "shipCountry")]
        public string ShipCountry { get; set; }
    }
}