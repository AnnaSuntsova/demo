using System.Xml;

namespace LibrarySystem
{
    class NewspaperParser
    {
        public ICatalogEntity ReadNewspapers(XmlReader reader)
        {
            int day, month, year;
            day = 0;
            month = 0;
            year = 0;
            if (reader.Name != "name")
            {
                reader.ReadToFollowing("name");
            }
            var name = reader.ReadElementContentAsString();
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
            if (reader.Name != "number")
            {
                reader.ReadToFollowing("number");
            }
            var number = reader.ReadElementContentAsInt();
            if (reader.Name != "date")
            {
                reader.ReadToFollowing("date");
            }
            if (reader.Name != "day")
            {
                reader.ReadToFollowing("day");
                day = reader.ReadElementContentAsInt();
            }
            if (reader.Name != "month")
            {
                reader.ReadToFollowing("month");                    
            }
            month = reader.ReadElementContentAsInt();
            if (reader.Name != "year")
            {
                reader.ReadToFollowing("year");
            }
            year = reader.ReadElementContentAsInt();    
            //var date = reader.ReadElementContentAsDateTime();
            if (reader.Name != "ISSN")
            {
                reader.ReadToFollowing("ISSN");
            }
            var issn = reader.ReadElementContentAsString();
            var newspaper = new Newspaper(name, publicationPlace, publisher, publicationYear, pageCount, notes, number, year, month, day, issn);
            return newspaper;
        }
    }
}
