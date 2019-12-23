using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LibrarySystem
{
    class Actions
    {
        //public string Name { get; set; }
        //public string[] Authors { get; set; }
        //public string PublicationPlace { get; set; }
        //public string Publisher { get; set; }
        //public int PublicationYear { get; set; }
        //public int pageCount { get; set; }
        //public string Notes { get; set; }
        //public string ISBN { get; set; }
        public void ReadBooks ()
        {
            var reader = XmlReader.Create("library.xml");
            reader.ReadToFollowing("books");
            while (reader.ReadToNextSibling("book"))
            {
                reader.ReadToDescendant("name");
                //output
                while (reader.ReadToNextSibling("authors"))
                {
                    reader.ReadToDescendant("author");
                }
                reader.ReadToDescendant("publicationPlace");
                //output
                reader.ReadToDescendant("publisher");
                //output
                reader.ReadToDescendant("publicationYear");
                //output
                reader.ReadToDescendant("pageCount");
                //output
                reader.ReadToDescendant("notes");
                //output
                reader.ReadToDescendant("ISBN");
                //output
            }
        }

        //public string Name { get; set; }
        //public string PublicationPlace { get; set; }
        //public string Publisher { get; set; }
        //public int PublicationYear { get; set; }
        //public int PageCount { get; set; }
        //public string Notes { get; set; }
        //public int Number { get; set; }
        //public DateTime Date { get; set; }
        //public string ISSN { get; set; }
        public void ReadNewspapers()
        {
            var reader = XmlReader.Create("library.xml");
            reader.ReadToFollowing("newspapers");
            while (reader.ReadToNextSibling("newspaper"))
            {
                reader.ReadToDescendant("name");
                //output                
                reader.ReadToDescendant("publicationPlace");
                //output
                reader.ReadToDescendant("publisher");
                //output
                reader.ReadToDescendant("publicationYear");
                //output
                reader.ReadToDescendant("pageCount");
                //output
                reader.ReadToDescendant("notes");
                //output
                reader.ReadToDescendant("number");
                //output
                reader.ReadToDescendant("date");
                //output
                reader.ReadToDescendant("ISSN");
                //output
            }
        }

        //public string Name { get; set; }
        //public string[] Inventors { get; set; }
        //public string City { get; set; }
        //public int RegistrationNumber { get; set; }
        //public DateTime ApplicationDate { get; set; }
        //public DateTime publicationDate { get; set; }
        //public int PageCount { get; set; }
        //public string Notes { get; set; }
        public void ReadPatents()
        {
            var reader = XmlReader.Create("library.xml");
            reader.ReadToFollowing("newspapers");
            while (reader.ReadToNextSibling("newspaper"))
            {
                reader.ReadToDescendant("Name");
                //output    
                while (reader.ReadToNextSibling("inventors"))
                {
                    reader.ReadToDescendant("inventors");
                }
                reader.ReadToDescendant("city");
                //output
                reader.ReadToDescendant("registrationNumber");
                //output
                reader.ReadToDescendant("applicationDate");
                //output
                reader.ReadToDescendant("publicationDate");
                //output
                reader.ReadToDescendant("pageCount");
                //output
                reader.ReadToDescendant("notes");
                //output
            }
        }

        //public string Name { get; set; }
        //public string[] Authors { get; set; }
        //public string PublicationPlace { get; set; }
        //public string Publisher { get; set; }
        //public int PublicationYear { get; set; }
        //public int pageCount { get; set; }
        //public string Notes { get; set; }
        //public string ISBN { get; set; }
        public void WriteBooks ()
        {
            var book = new Book();
            using (XmlWriter writer = XmlWriter.Create("books.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("books");
                writer.WriteStartElement("book");
                writer.WriteElementString("name", book.Name);
                writer.WriteElementString("author", book.Authors[0]);
                writer.WriteElementString("publicationPlace", book.PublicationPlace);
                writer.WriteElementString("publisher", book.Publisher);
                writer.WriteElementString("publicationYear", book.PublicationYear.ToString());
                writer.WriteElementString("pageCount", book.PageCount.ToString());
                writer.WriteElementString("notes", book.Notes);
                writer.WriteElementString("ISBN", book.ISBN);
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        //public string Name { get; set; }
        //public string PublicationPlace { get; set; }
        //public string Publisher { get; set; }
        //public int PublicationYear { get; set; }
        //public int PageCount { get; set; }
        //public string Notes { get; set; }
        //public int Number { get; set; }
        //public DateTime Date { get; set; }
        //public string ISSN { get; set; }
        public void WriteNewspapers()
        {
            var newspapers = new Newspaper();
            using (XmlWriter writer = XmlWriter.Create("newspapers.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("newspapers");
                writer.WriteStartElement("newspaper");
                writer.WriteElementString("name", newspapers.Name);
                writer.WriteElementString("publicationPlace", newspapers.PublicationPlace);
                writer.WriteElementString("publisher", newspapers.Publisher);
                writer.WriteElementString("pageCount", newspapers.PageCount.ToString());
                writer.WriteElementString("publicationYear", newspapers.PublicationYear.ToString());
                writer.WriteElementString("pageCount", newspapers.PageCount.ToString());
                writer.WriteElementString("notes", newspapers.Notes.ToString());
                writer.WriteElementString("date", newspapers.Date.ToString());
                writer.WriteElementString("ISSN", newspapers.ISSN);
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        //public string Name { get; set; }
        //public string[] Inventors { get; set; }
        //public string City { get; set; }
        //public int RegistrationNumber { get; set; }
        //public DateTime ApplicationDate { get; set; }
        //public DateTime publicationDate { get; set; }
        //public int PageCount { get; set; }
        //public string Notes { get; set; }
        public void WritePatents()
        {
            var patent = new Patent();
            using (XmlWriter writer = XmlWriter.Create("books.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("patents");
                writer.WriteStartElement("patent");
                writer.WriteElementString("name", patent.Name);
                writer.WriteElementString("inventor", patent.Inventors[0]);
                writer.WriteElementString("city", patent.City);
                writer.WriteElementString("registrationNumber", patent.RegistrationNumber.ToString());
                writer.WriteElementString("applicationDate", patent.ApplicationDate.ToString());
                writer.WriteElementString("publicationDate", patent.PublicationDate.ToString());
                writer.WriteElementString("pageCount", patent.PageCount.ToString());
                writer.WriteElementString("notes", patent.Notes);
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        private void CheckValidation()
        {
            XmlReaderSettings catalogSettings = new XmlReaderSettings();
            catalogSettings.Schemas.Add();

        }


    }
}
