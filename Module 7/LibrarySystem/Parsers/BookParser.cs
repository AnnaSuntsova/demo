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
        public ICatalogEntity ReadBooks()
        {
            const string nameOfFile = "outputXML.xml";
            var reader = XmlReader.Create(nameOfFile);
            reader.ReadToFollowing("books");
            
            reader.ReadToDescendant("name");
            var name = reader.ReadElementContentAsString();
            var authors = new List<string>();
            while (reader.ReadToNextSibling("authors"))
            {
                reader.ReadToDescendant("author");
                authors.Add(reader.ReadElementContentAsString());
            }
            reader.ReadToDescendant("publicationPlace");
            var publicationPlace = reader.ReadElementContentAsString();
            reader.ReadToDescendant("publisher");
            var publisher = reader.ReadElementContentAsString();
            reader.ReadToDescendant("publicationYear");
            var publicationYear = reader.ReadElementContentAsInt();
            reader.ReadToDescendant("pageCount");
            var pageCount = reader.ReadElementContentAsInt();
            reader.ReadToDescendant("notes");
            var notes = reader.ReadElementContentAsString();
            reader.ReadToDescendant("ISBN");
            var isbn = reader.ReadElementContentAsString();
            var book = new Book(name, authors, publicationPlace, publisher, publicationYear, pageCount, notes, isbn);

            return book;
        }
    }
}
