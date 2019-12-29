using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace LibrarySystem
{
    class BookParser
    {      
        public ICatalogEntity ReadBooks(XmlReader reader)
        {
            if (reader.Name != "name")
            {
                reader.ReadToFollowing("name");
            }
            var name = reader.ReadElementContentAsString();
            if (reader.Name != "author")
            {
                reader.ReadToFollowing("author");
            }
            var authors = new List<string>();
            while (reader.Name=="author")
            {
                authors.Add(reader.ReadElementContentAsString());
            }
            if (reader.Name != "publicationPlace")
            {
                reader.ReadToFollowing("publicationPlace");
            }
            var publicationPlace = reader.ReadElementContentAsString();
            if (reader.Name != "publisher")
            {
                reader.ReadToFollowing("publisher");
            }
            var publisher = reader.ReadElementContentAsString();
            if (reader.Name != "publicationYear")
            {
                reader.ReadToFollowing("publicationYear");
            }
            var publicationYear = reader.ReadElementContentAsInt();
            if (reader.Name != "pageCount")
            {
                reader.ReadToFollowing("pageCount");
            }
            var pageCount = reader.ReadElementContentAsInt();
            if (reader.Name != "notes")
            {
                reader.ReadToFollowing("notes");
            }
            var notes = reader.ReadElementContentAsString();
            if (reader.Name != "ISBN")
            {
                reader.ReadToFollowing("ISBN");
            }
            var isbn = reader.ReadElementContentAsString();
            var book = new Book(name, authors, publicationPlace, publisher, publicationYear, pageCount, notes, isbn);
            return book;               
        }
    }
}
