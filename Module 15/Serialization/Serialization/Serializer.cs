using Serialization.Entities;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Serialization
{
    public class Serializer
    {
        public void Serialize(Catalog catalog, string fileName, Encoding encoding)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Catalog));
            using (var xmlWriter = XmlWriter.Create(fileName, new XmlWriterSettings
            {
                Encoding = encoding,
                Indent = true
            }))
            {
                serializer.Serialize(xmlWriter, catalog);
            }
        }
    }
}
