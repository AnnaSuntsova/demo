using System;
using System.Linq;
using System.Xml.Linq;

namespace LibrarySystem
{
    class NewspaperParser
    {
        public void ReadNewspapers(string reader)
        {
            var newspaper = new Newspaper();
            var xDoc = XElement.Parse(reader);

            foreach (var item in xDoc.Elements("newspapers"))
            {
                var xNode = XElement.Parse(item.ToString());
                foreach (var node in xNode.Elements("newspaper"))
                {
                    try
                    {
                        Library.libraryNewspapers.Add(
                        newspaper = new Newspaper
                        {
                            Name = node.Elements("name").FirstOrDefault()?.Value,
                            Publisher = node.Elements("publisher").FirstOrDefault()?.Value,
                            PublicationPlace = node.Elements("publicationPlace").FirstOrDefault()?.Value,
                            PageCount = int.Parse(node.Elements("pageCount").FirstOrDefault()?.Value),
                            PublicationYear = int.Parse(node.Elements("publicationYear").FirstOrDefault()?.Value),
                            Notes = node.Elements("notes").FirstOrDefault()?.Value,
                            Number = int.Parse(node.Elements("number").FirstOrDefault()?.Value),
                            Date = new DateTime(int.Parse(node.Elements("date").Descendants().Where(x => x.Name == "year").FirstOrDefault()?.Value),
                                                int.Parse(node.Elements("date").Descendants().Where(x => x.Name == "month").FirstOrDefault()?.Value),
                                                int.Parse(node.Elements("date").Descendants().Where(x => x.Name == "day").FirstOrDefault()?.Value)),
                            ISSN = node.Elements("issn").FirstOrDefault()?.Value
                        });
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
            }         
        }
    }
}
