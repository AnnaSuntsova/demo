using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace LibrarySystem
{
    public class Actions
    {
        private List<Book> books = new List<Book>();
        private List<Newspaper> newspapers = new List<Newspaper>();
        private List<Patent> patents = new List<Patent>();

        public void ReadBooks ()
        {
            var reader = XmlReader.Create("library.xml");
            reader.ReadToFollowing("books");
            while (reader.ReadToNextSibling("book"))
            {
                reader.ReadToDescendant("name");
                var name = reader.ReadElementContentAsString();
                var authors = new List<string>();
                while (reader.ReadToNextSibling("authors"))
                {
                    reader.ReadToDescendant("author");
                    authors.Add(reader.ReadElementContentAsString());
                }
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
                reader.ReadToDescendant("ISBN");
                var isbn = reader.ReadElementContentAsString();
                var book = new Book(name, authors, publicationPlace, publisher, publicationYear, pageCount, notes, isbn);
                books.Add(book);
            }
        }

        public void ReadNewspapers()
        {
            var reader = XmlReader.Create("library.xml");
            reader.ReadToFollowing("newspapers");
            while (reader.ReadToNextSibling("newspaper"))
            {
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
                newspapers.Add(newspaper);
            }
        }

        public void ReadPatents()
        {
            var reader = XmlReader.Create("library.xml");
            reader.ReadToFollowing("newspapers");
            while (reader.ReadToNextSibling("newspaper"))
            {
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
                patents.Add(patent);
            }
        }

        public void WriteBooks ()
        {
            var book = new Book();
            using (XmlWriter writer = XmlWriter.Create("books.xml"))
            {
                writer.WriteStartDocument();
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

        public void WriteNewspapers()
        {
            var newspapers = new Newspaper();
            using (XmlWriter writer = XmlWriter.Create("newspapers.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("newspaper");
                writer.WriteElementString("name", newspapers.Name);
                writer.WriteElementString("publicationPlace", newspapers.PublicationPlace);
                writer.WriteElementString("publisher", newspapers.Publisher);
                writer.WriteElementString("pageCount", newspapers.PageCount.ToString());
                writer.WriteElementString("publicationYear", newspapers.PublicationYear.ToString());
                writer.WriteElementString("pageCount", newspapers.PageCount.ToString());
                writer.WriteElementString("notes", newspapers.Notes);
                writer.WriteElementString("date", newspapers.Date.ToString());
                writer.WriteElementString("ISSN", newspapers.ISSN);
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public void WritePatents()
        {
            var patent = new Patent();
            using (XmlWriter writer = XmlWriter.Create("books.xml"))
            {
                writer.WriteStartDocument();
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

        public void CheckValidation()
        {
            var catalogSettings = new XmlReaderSettings();
            catalogSettings.Schemas.Add("urn:schemas-microsoft-com:xml-msdata", @"C:\data\git_demo\demo\Module 7\LibrarySystem\catalog.xsd");
            catalogSettings.ValidationType = ValidationType.Schema;
            var catalog = XmlReader.Create(@"C:\data\git_demo\demo\Module 7\inputFile.xml", catalogSettings);
            var document = new XmlDocument();
            document.Load(catalog);
            ValidationEventHandler eventHandler = CatalogSettingsValidationEventHandler;
            document.Validate(eventHandler);
        }

        static void CatalogSettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                Console.Write("Warning: ");
                Console.WriteLine(e.Message);
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                Console.Write("Error: ");
                Console.WriteLine(e.Message);
            }
        }
    }
}
