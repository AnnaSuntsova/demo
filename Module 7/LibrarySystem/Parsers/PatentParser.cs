using System.Collections.Generic;
using System.Xml;

namespace LibrarySystem
{
    class PatentParser
    {
        public ICatalogEntity ReadPatents()
        {
            const string nameOfFile = "outputXML.xml";
            var reader = XmlReader.Create(nameOfFile);
            reader.ReadToFollowing("patent");

            reader.ReadToDescendant("Name");
            var name = reader.ReadElementContentAsString();
            var inventors = new List<string>();
            while (reader.ReadToNextSibling("inventors"))
            {
                reader.ReadToDescendant("inventors");
                inventors.Add(reader.ReadElementContentAsString());
            }
            reader.ReadToDescendant("city");
            var city = reader.ReadElementContentAsString();
            reader.ReadToDescendant("registrationNumber");
            var registrationNumber = reader.ReadElementContentAsInt();
            reader.ReadToDescendant("applicationDate");
            var applicationDate = reader.ReadElementContentAsDateTime();
            reader.ReadToDescendant("publicationDate");
            var publicationDate = reader.ReadElementContentAsDateTime();
            reader.ReadToDescendant("pageCount");
            var pageCount = reader.ReadElementContentAsInt();
            reader.ReadToDescendant("notes");
            var notes = reader.ReadElementContentAsString();

            var patent = new Patent(name, inventors, city, registrationNumber, applicationDate, publicationDate, pageCount, notes);
            return patent;
        }
    }
}
