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
            if (!string.IsNullOrWhiteSpace(newspaper.Name))
            {
                writer.WriteElementString("name", newspaper.Name);
            }
            if (!string.IsNullOrWhiteSpace(newspaper.PublicationPlace))
            {
                writer.WriteElementString("publicationPlace", newspaper.PublicationPlace);
            }
            if (!string.IsNullOrWhiteSpace(newspaper.Publisher))
            {
                writer.WriteElementString("publisher", newspaper.Publisher);
            }
            if (newspaper.PageCount!=0)
            {
                writer.WriteElementString("pageCount", newspaper.PageCount.ToString());
            }
            if (newspaper.PublicationYear != 0)
            {
                writer.WriteElementString("publicationYear", newspaper.PublicationYear.ToString());
            }
            if (!string.IsNullOrWhiteSpace(newspaper.Notes))
            {
                writer.WriteElementString("notes", newspaper.Notes);
            }
            if (newspaper.Number != 0)
            {
                writer.WriteElementString("number", newspaper.Number.ToString());
            }
            if (newspaper.Date!=DateTime.MinValue)
            {
                writer.WriteElementString("date", newspaper.Date.ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrWhiteSpace(newspaper.ISSN))
            {
                writer.WriteElementString("ISSN", newspaper.ISSN);
            }
            writer.WriteEndElement();
        }
    }
}
