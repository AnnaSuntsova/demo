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
            if (!string.IsNullOrWhiteSpace(book.Name))
            {
                writer.WriteElementString("name", book.Name);
            }
            if (book.Authors.Count != 0)
            {
                writer.WriteStartElement("authors");
                foreach (var author in book.Authors)
                {
                    writer.WriteElementString("author", author);
                }
                writer.WriteEndElement();
            }
            if (!string.IsNullOrWhiteSpace(book.PublicationPlace))
            {
                writer.WriteElementString("publicationPlace", book.PublicationPlace);
            }
            if (!string.IsNullOrWhiteSpace(book.Publisher))
            {
                writer.WriteElementString("publisher", book.Publisher);
            }
            if (book.PublicationYear != 0)
            {
                writer.WriteElementString("publicationYear", book.PublicationYear.ToString());
            }
            if (book.PageCount!=0)
            {
                writer.WriteElementString("pageCount", book.PageCount.ToString());
            }
            if (!string.IsNullOrWhiteSpace(book.Notes))
            {
                writer.WriteElementString("notes", book.Notes);
            }
            if (!string.IsNullOrWhiteSpace(book.ISBN))
            {
                writer.WriteElementString("ISBN", book.ISBN);
            }
            writer.WriteEndElement();
        }
    }
}
