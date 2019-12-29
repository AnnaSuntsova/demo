using System.Collections.Generic;
using System.Xml;

namespace LibrarySystem
{
    class PatentParser
    {
        public ICatalogEntity ReadPatents(XmlReader reader)
        {
            if (reader.Name != "name")
            {
                reader.ReadToFollowing("name");
            }
            var name = reader.ReadElementContentAsString();
            if (reader.Name != "inventor")
            {
                reader.ReadToFollowing("inventor");
            }
            var inventors = new List<string>();
            while (reader.Name == "inventor")
            {
                inventors.Add(reader.ReadElementContentAsString());
            }
            if (reader.Name != "city")
            {
                reader.ReadToFollowing("city");
            }
            var city = reader.ReadElementContentAsString();
            if (reader.Name != "registrationNumber")
            {
                reader.ReadToFollowing("registrationNumber");
            }
            var registrationNumber = reader.ReadElementContentAsInt();
            if (reader.Name != "applicationDate")
            {
                reader.ReadToFollowing("applicationDate");
            }
            var applicationDate = reader.ReadElementContentAsDateTime();
            if (reader.Name != "publicationDate")
            {
                reader.ReadToFollowing("publicationDate");
            }
            var publicationDate = reader.ReadElementContentAsDateTime();
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
            var patent = new Patent(name, inventors, city, registrationNumber, applicationDate, publicationDate, pageCount, notes);
            return patent;
        }
    }
}
