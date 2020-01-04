using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace LibrarySystem
{
    class BookWriter
    {
        public void WriteBooks(IEnumerable<Book>books, XmlWriter writer)
        {
            var element = new XElement("books",
               books.Select(
                   book => new XElement("book",
             new XElement("name", book.Name),
             new XElement("authors", book.Authors.Select(x => new XElement("author", x))),
             new XElement("publicationPlace", book.PublicationPlace),
             new XElement("publisher", book.Publisher),
             new XElement("publicationYear", book.PublicationYear),
             new XElement("pageCount", book.PageCount),
             new XElement("notes", book.Notes),
             new XElement("isbn", book.ISBN)
             )));
             element.WriteTo(writer);
        }
    }
}
