using LibrarySystem.Entities;
using System;

namespace LibrarySystem
{
    public class Newspaper: ICatalogEntity
    {
        public string Name { get; set; }
        public string PublicationPlace { get; set; }
        public string Publisher { get; set; }
        public int PublicationYear { get; set; }
        public int PageCount { get; set; }
        public string Notes { get; set; }
        public int Number { get; set; }
        public Date Date { get; set; }
        public string ISSN { get; set; }

        public Newspaper(string name, string publicationPlace, string publisher, int publicationYear, int pageCount, string notes, int number, int year, int month, int day, string issn)
        {
            if (string.IsNullOrWhiteSpace(issn))
            {
                throw new ArgumentOutOfRangeException();
            }
            if (month>12 && (month<0))
            {
                throw new ArgumentOutOfRangeException();
            }
            if (day > 31 && (day < 0))
            {
                throw new ArgumentOutOfRangeException();
            }
            Name = name;
            PublicationPlace = publicationPlace;
            Publisher = publisher;
            PublicationYear = publicationYear;
            PageCount = pageCount;
            Notes = notes;
            Number = number;
            Date = new Date
            {
                Year = year,
                Month = month,
                Day = day
            };
            ISSN = issn;
        }

        public Newspaper()
        { }
    }
}
