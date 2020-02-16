using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Serialization.Entities
{
    [Serializable]
    [XmlRoot(ElementName = "catalog", Namespace = "http://library.by/catalog")]
    public class Catalog
    {
        [XmlAttribute(AttributeName = "date", DataType = "date")]
        public DateTime Date { get; set; }
        [XmlElement(ElementName = "book")]
        public List<Book> Books { get; set; }
    }
}
