using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace LibrarySystem
{
    class NewspaperWriter
    {
        public void WriteNewspapers(Newspaper newspaper, XmlWriter writer)
        {
            writer.WriteStartElement("newspaper");
            writer.WriteElementString("name", newspaper.Name);
            writer.WriteElementString("publicationPlace", newspaper.PublicationPlace);
            writer.WriteElementString("publisher", newspaper.Publisher);
            writer.WriteElementString("pageCount", newspaper.PageCount.ToString());
            writer.WriteElementString("publicationYear", newspaper.PublicationYear.ToString());
            writer.WriteElementString("pageCount", newspaper.PageCount.ToString());
            writer.WriteElementString("notes", newspaper.Notes);
            writer.WriteElementString("date", newspaper.Date.ToString());
            writer.WriteElementString("ISSN", newspaper.ISSN);
            writer.WriteEndElement();
        }
    }
}
