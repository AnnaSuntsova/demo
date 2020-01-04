using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace LibrarySystem
{
    class NewspaperWriter
    {
        public void WriteNewspapers(IEnumerable<Newspaper> newspapers, XmlWriter writer)
        {
            var element = new XElement("newspapers",
               newspapers.Select(
                   newspaper => new XElement("newspaper",            
            new XElement("name", newspaper.Name),
            new XElement("publicationPlace", newspaper.PublicationPlace),
            new XElement("publisher", newspaper.Publisher),
            new XElement("pageCount", newspaper.PageCount),
            new XElement("publicationYear", newspaper.PublicationYear),
            new XElement("notes", newspaper.Notes),
            new XElement("number", newspaper.Number),
            new XElement("date", new XElement("day", newspaper.Date.Day),
                                 new XElement("month", newspaper.Date.Month),
                                 new XElement("year", newspaper.Date.Year)),
            new XElement("issn", newspaper.ISSN)
            )));
            element.WriteTo(writer);            
        }
    }
}
