using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace LibrarySystem
{
    public class Lybrary
    {
        private string _nameOfElement = "catalog";
        public event EventHandler OnWarning;
        public event EventHandler OnError;

        public IEnumerable<ICatalogEntity> Read (TextReader input)
        {
           XmlReader xmlReader = XmlReader.Create(input);
           {
                xmlReader.MoveToContent();
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {                   

                        if (xmlReader.Name == "book")
                        {
                            BookParser parser = new BookParser();
                            yield return parser.ReadBooks(xmlReader);
                        }
                        else if (xmlReader.Name == "newspaper")
                        {
                            NewspaperParser parser = new NewspaperParser();
                            yield return parser.ReadNewspapers(xmlReader);
                        }
                        else if (xmlReader.Name == "patent")
                        {
                            PatentParser parser = new PatentParser();
                            yield return parser.ReadPatents(xmlReader);
                        }
                    }
                }
            }
        }

        public void WriteTo(TextWriter output, IEnumerable<ICatalogEntity> catalogEntities)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(output, new XmlWriterSettings()))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement(_nameOfElement);
                foreach (var catalogEntity in catalogEntities)
                {
                    if (catalogEntity.GetType().Equals(typeof(Book)))
                    {
                        var bookWriter = new BookWriter();
                        bookWriter.WriteBooks((Book)catalogEntity, xmlWriter);
                    }
                    else 
                    if (catalogEntity.GetType().Equals(typeof(Newspaper)))
                    {
                        var newspaperWriter = new NewspaperWriter();
                        newspaperWriter.WriteNewspapers((Newspaper)catalogEntity, xmlWriter);
                    }
                    else
                    if (catalogEntity.GetType().Equals(typeof(Patent)))
                    {
                        var patentWriter = new PatentWriter();
                        patentWriter.WritePatents((Patent)catalogEntity, xmlWriter);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                xmlWriter.WriteEndElement();
            }
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
