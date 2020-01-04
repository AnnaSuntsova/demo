using System.Linq;
using System.Xml.Linq;


namespace LibrarySystem
{
    class BookParser
    {      
        public void ReadBooks(string reader)
        {
            var book = new Book();
            var xDoc = XElement.Parse(reader);

            foreach (var item in xDoc.Elements("books"))
            {
                var xNode = XElement.Parse(item.ToString());
                foreach (var node in xNode.Elements("book"))
                {
                    Library.libraryBooks.Add(
                    book = new Book
                    {
                        Name = node.Elements("name").FirstOrDefault()?.Value,
                        Authors = node.Elements("authors").Descendants()?.Select(x => x.Value.ToString()).ToList(),
                        Publisher = node.Elements("publisher").FirstOrDefault()?.Value,
                        PublicationPlace = node.Elements("publicationPlace").FirstOrDefault()?.Value,
                        PageCount = int.Parse(node.Elements("pageCount").FirstOrDefault()?.Value),
                        PublicationYear = int.Parse(node.Elements("publicationYear").FirstOrDefault()?.Value),
                        Notes = node.Elements("notes").FirstOrDefault()?.Value,
                        ISBN = node.Elements("isbn").FirstOrDefault()?.Value,
                    });
                }
            }
        }
    }
}
