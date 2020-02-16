using Serialization.Entities;
using System.IO;
using System.Xml.Serialization;

namespace Serialization
{
    public class Deserializer
    {
        Catalog catalog;

        public Catalog Deserialize(string nameOfFile, FileMode mode)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Catalog));
            using (FileStream fs = new FileStream(nameOfFile, mode))
            {
                catalog = (Catalog)serializer.Deserialize(fs);
            }
            return catalog;
        }
    }
}
