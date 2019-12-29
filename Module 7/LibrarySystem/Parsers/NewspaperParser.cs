using System.Xml;

namespace LibrarySystem
{
    class NewspaperParser
    {
        public ICatalogEntity ReadNewspapers(XmlReader reader)
        {
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
            var date = reader.ReadElementContentAsDateTime();
            if (reader.Name != "ISSN")
            {
                reader.ReadToFollowing("ISSN");
            }
            var issn = reader.ReadElementContentAsString();
            var newspaper = new Newspaper(name, publicationPlace, publisher, publicationYear, pageCount, notes, number, date, issn);
            return newspaper;
        }
    }
}
