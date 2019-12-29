﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace LibrarySystem
{
    public class Actions
    {
        private string _nameOfElement = "catalog";

        public IEnumerable<ICatalogEntity> Read (TextReader input)
        {
           XmlReader xmlReader = XmlReader.Create(input);
           {
                xmlReader.MoveToContent();
                //xmlReader.ReadToFollowing(_nameOfElement);
                //xmlReader.ReadStartElement();
                //do
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {                   

                        if (xmlReader.Name == "book")
                        {
                            //var node = XElement.ReadFrom(xmlReader) as XElement;
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
                //} while (xmlReader.Read());
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
