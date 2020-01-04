using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace LibrarySystem
{
    public class Library
    {
        private string _nameOfElement = "catalog";
        public event EventHandler OnWarning;
        public event EventHandler OnError;
        public static List<Book> libraryBooks = new List<Book>();
        public static List<Newspaper> libraryNewspapers = new List<Newspaper>();
        public static List<Patent> libraryPatents = new List<Patent>();

        public void Read (string input)
        {
           XmlReader xmlReader = XmlReader.Create(new StringReader (input));
           {
                xmlReader.MoveToContent();
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {              
                        if (xmlReader.Name == "books")
                        {
                            BookParser parser = new BookParser();
                            parser.ReadBooks(input);
                        }
                        else if (xmlReader.Name == "newspapers")
                        {
                            NewspaperParser parser = new NewspaperParser();
                            parser.ReadNewspapers(input);
                        }
                        else if (xmlReader.Name == "patents")
                        {
                            PatentParser parser = new PatentParser();
                            parser.ReadPatents(input);
                        }
                    }
                }
            }
        }

        public TextWriter WriteTo(IEnumerable<ICatalogEntity>[] catalogEntities)
        {
            TextWriter output = new StringWriter();
            using (XmlWriter xmlWriter = XmlWriter.Create(output, new XmlWriterSettings()))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement(_nameOfElement);
                foreach (var catalogEntity in catalogEntities)
                {                    
                    if (catalogEntity.GetType().Equals(typeof(List<Book>)))
                    {
                        var bookWriter = new BookWriter();
                        bookWriter.WriteBooks((IEnumerable<Book>) catalogEntity, xmlWriter);
                    }
                    else
                    if (catalogEntity.GetType().Equals(typeof(List<Newspaper>)))
                    {
                        var newspaperWriter = new NewspaperWriter();
                        newspaperWriter.WriteNewspapers((IEnumerable<Newspaper>)catalogEntity, xmlWriter);
                    }
                    else
                    if (catalogEntity.GetType().Equals(typeof(List<Patent>)))
                    {
                        var patentWriter = new PatentWriter();
                        patentWriter.WritePatents((IEnumerable<Patent>)catalogEntity, xmlWriter);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                xmlWriter.WriteEndElement();
            }
            return output;
        }       

        public void CheckValidation(XmlReader catalog)
        {
            var catalogSettings = new XmlReaderSettings();
            var pathToXsd = Path.Combine(Environment.CurrentDirectory, "catalog.xsd");
            catalogSettings.Schemas.Add("urn:schemas-microsoft-com:xml-msdata", pathToXsd);
            catalogSettings.ValidationType = ValidationType.Schema;
            var document = new XmlDocument();
            document.Load(catalog);
            ValidationEventHandler eventHandler = CatalogSettingsValidationEventHandler;
            document.Validate(eventHandler);
        }

        private void CatalogSettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                OnWarning?.Invoke(this, null);
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                OnError?.Invoke(this, null);
            }
        }
    }
}
