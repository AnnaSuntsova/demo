using System;
using System.Collections.Generic;

namespace LibrarySystem
{
    public class Book: ICatalogEntity
    {
        public string Name { get; set; }
        public List<string> Authors { get; set; }
        public string PublicationPlace { get; set; }
        public string Publisher { get; set; }
        public int PublicationYear { get; set; }
        public int PageCount { get; set; }
        public string Notes { get; set; }
        public string ISBN { get; set; }

        public Book(string name, List<string> authors, string publicationPlace, string publisher, int publicationYear, int pageCount, string notes, string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentOutOfRangeException();
            }

            Name = name;
            foreach (var author in authors)
            {
                Authors.Add(author);
            }
            PublicationPlace = publicationPlace;
            Publisher = publisher;
            PublicationYear = publicationYear;
            PageCount = pageCount;
            Notes = notes;
            ISBN = isbn;
        }

        public Book()
        { }
    }    
}
