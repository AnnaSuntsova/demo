using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace LibrarySystem
{
    class BookWriter
    {
        public void WriteBooks(Book book, XmlWriter writer)
        {               
            writer.WriteStartElement("book");
            writer.WriteElementString("name", book.Name);
            foreach (var author in book.Authors)
            {
                writer.WriteElementString("author", author);
            }                
            writer.WriteElementString("publicationPlace", book.PublicationPlace);
            writer.WriteElementString("publisher", book.Publisher);
            writer.WriteElementString("publicationYear", book.PublicationYear.ToString());
            writer.WriteElementString("pageCount", book.PageCount.ToString());
            writer.WriteElementString("notes", book.Notes);
            writer.WriteElementString("ISBN", book.ISBN);
            writer.WriteEndElement();
        }
    }
}
