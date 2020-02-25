using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace HttpHandlerForNorthwind
{
    public class Converters
    {
        public void ToXmlFormat(IEnumerable<OrderFields> orders, MemoryStream stream)
        {
            var serializer = new XmlSerializer(typeof(List<OrderFields>));
            serializer.Serialize(stream, orders);
        }

        public IEnumerable<OrderFields> FromXmlFormat(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(List<OrderFields>));
            return serializer.Deserialize(stream) as List<OrderFields>;
        }

        public void ToExcel(IEnumerable<OrderFields> orders, MemoryStream stream)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("OrderInfo");
            worksheet.Cell("A" + 1).Value = "Customer";
            worksheet.Cell("B" + 1).Value = "OrderDate";
            worksheet.Cell("C" + 1).Value = "Freight";
            worksheet.Cell("D" + 1).Value = "ShipName";
            worksheet.Cell("E" + 1).Value = "ShipCountry";
            worksheet.Cell("A" + 2).InsertData(orders);
            worksheet.Columns().AdjustToContents();
            workbook.SaveAs(stream);
        }
    }
}