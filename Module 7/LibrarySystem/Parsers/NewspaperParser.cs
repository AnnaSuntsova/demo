using System.Xml;

namespace LibrarySystem
{
    class NewspaperParser
    {
        public ICatalogEntity ReadNewspapers()
        {
            const string nameOfFile = "outputXML.xml";
            var reader = XmlReader.Create(nameOfFile);
            reader.ReadToDescendant("name");
            var name = reader.ReadElementContentAsString();
            reader.ReadToDescendant("publicationPlace");
            var publicationPlace = reader.ReadElementContentAsString();
            reader.ReadToDescendant("publisher");
            var publisher = reader.ReadElementContentAsString();
            reader.ReadToDescendant("publicationYear");
            var publicationYear = reader.ReadElementContentAsInt();
            reader.ReadToDescendant("pageCount");
            var pageCount = reader.ReadElementContentAsInt();
            reader.ReadToDescendant("notes");
            var notes = reader.ReadElementContentAsString();
            reader.ReadToDescendant("number");
            var number = reader.ReadElementContentAsInt();
            reader.ReadToDescendant("date");
            var date = reader.ReadElementContentAsDateTime();
            reader.ReadToDescendant("ISSN");
            var issn = reader.ReadElementContentAsString();
            var newspaper = new Newspaper(name, publicationPlace, publisher, publicationYear, pageCount, notes, number, date, issn);
            return newspaper;
        }

    }
}
