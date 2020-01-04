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
        public DateTime Date { get; set; }
        public string ISSN { get; set; }

        public Newspaper(string name, string publicationPlace, string publisher, int publicationYear, int pageCount, string notes, int number, int year, int month, int day, string issn)
        {
            if (string.IsNullOrWhiteSpace(issn))
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
            try
            {
                Date = new DateTime(year, month, day);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException("Date is out of range");
            }
            ISSN = issn;
        }

        public Newspaper()
        { }
    }
}
