using System;
using System.Xml;

namespace LibrarySystem
{
    class PatentWriter
    {
        public void WritePatents(Patent patent, XmlWriter writer)
        {
            writer.WriteStartElement("patent");
            if (!string.IsNullOrWhiteSpace(patent.Name))
            {
                writer.WriteElementString("name", patent.Name);
            }
            if (patent.Inventors.Count != 0)
            {
                writer.WriteStartElement("inventors");
                foreach (var inventor in patent.Inventors)
                {
                    writer.WriteElementString("inventor", inventor);
                }
                writer.WriteEndElement();
            }
            if (!string.IsNullOrWhiteSpace(patent.City))
            {
                writer.WriteElementString("city", patent.City);
            }
            if (patent.RegistrationNumber!=0)
            {
                writer.WriteElementString("registrationNumber", patent.RegistrationNumber.ToString());
            }
            if (patent.ApplicationDate != DateTime.MinValue)
            {
                writer.WriteElementString("applicationDate", patent.ApplicationDate.ToString("yyyy-MM-dd"));
            }
            if (patent.PublicationDate != DateTime.MinValue)
            {
                writer.WriteElementString("publicationDate", patent.PublicationDate.ToString("yyyy-MM-dd"));
            }
            if (patent.PageCount != 0)
            {
                writer.WriteElementString("pageCount", patent.PageCount.ToString());
            }
            if (!string.IsNullOrWhiteSpace(patent.Notes))
            {
                writer.WriteElementString("notes", patent.Notes);
            }
            writer.WriteEndElement();
        }
    }
}
