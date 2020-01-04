using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace LibrarySystem
{
    class PatentWriter
    {
        public void WritePatents(IEnumerable<Patent> patents, XmlWriter writer)
        {
            var element = new XElement ("patents",
               patents.Select(patent => new XElement("patent",
            new XElement("name", patent.Name),
            new XElement("inventors", patent.Inventors.Select(x => new XElement("inventor", x))),
            new XElement("city", patent.City),
            new XElement("registrationNumber", patent.RegistrationNumber),            
            new XElement("applicationDate", new XElement("day", patent.ApplicationDate.Day),
                                 new XElement("month", patent.ApplicationDate.Month),
                                 new XElement("year", patent.ApplicationDate.Year)),
            new XElement("publicationDate", new XElement("day", patent.PublicationDate.Day),
                                 new XElement("month", patent.PublicationDate.Month),
                                 new XElement("year", patent.PublicationDate.Year)),
            new XElement("pageCount", patent.PageCount),
            new XElement("notes", patent.Notes)
            )));
            element.WriteTo(writer);
        }
    }
}
