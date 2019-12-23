using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem
{
    public class Newspaper
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

        public Newspaper(string name, string publicationPlace, string publisher, int publicationYear, int pageCount, string notes, int number, DateTime date, string issn)
        {
            if (string.IsNullOrWhiteSpace(issn))
            {
                throw new ArgumentOutOfRangeException();
            }
            else
            {
                Name = name;
                PublicationPlace = publicationPlace;
                Publisher = publisher;
                PublicationYear = publicationYear;
                PageCount = pageCount;
                Notes = notes;
                Number = number;
                Date = date;
                ISSN = issn;
            }
        }

        public Newspaper()
        { }
    }
}
