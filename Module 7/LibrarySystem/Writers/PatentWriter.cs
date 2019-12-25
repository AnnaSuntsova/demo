using System.Xml;

namespace LibrarySystem
{
    class PatentWriter
    {
        public void WritePatents(Patent patent, XmlWriter writer)
        {
            writer.WriteStartElement("patent");
            writer.WriteElementString("name", patent.Name);
            foreach (var inventor in patent.Inventors)
            {
                writer.WriteElementString("inventor", inventor);
            }
            writer.WriteElementString("city", patent.City);
            writer.WriteElementString("registrationNumber", patent.RegistrationNumber.ToString());
            writer.WriteElementString("applicationDate", patent.ApplicationDate.ToString());
            writer.WriteElementString("publicationDate", patent.PublicationDate.ToString());
            writer.WriteElementString("pageCount", patent.PageCount.ToString());
            writer.WriteElementString("notes", patent.Notes);
            writer.WriteEndElement();
        }
    }
}
