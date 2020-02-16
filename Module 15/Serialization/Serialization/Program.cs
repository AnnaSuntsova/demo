using System.IO;
using System.Text;

namespace Serialization
{
    class Program
    {
        static void Main(string[] args)
        {            
            var deserializer = new Deserializer();
            var element = deserializer.Deserialize("books.xml", FileMode.Open);

            var serializer = new Serializer();
            serializer.Serialize(element, "booksNew.xml", Encoding.UTF8);            
        }
    }
}
