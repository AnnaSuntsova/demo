using System;
using System.Linq;
using System.Xml.Linq;

namespace LibrarySystem
{
    class PatentParser
    {
        public void ReadPatents(string reader)
        {
            var patent = new Patent();
            var xDoc = XElement.Parse(reader);

            foreach (var item in xDoc.Elements("patents"))
            {
                var xNode = XElement.Parse(item.ToString());
                foreach (var node in xNode.Elements("patent"))
                {
                    try
                    {
                        Library.libraryPatents.Add(
                        patent = new Patent
                        {
                            Name = node.Elements("name").FirstOrDefault()?.Value,
                            Inventors = node.Elements("inventors").Descendants()?.Select(x => x.Value.ToString()).ToList(),
                            City = node.Elements("city").FirstOrDefault()?.Value,
                            PageCount = int.Parse(node.Elements("pageCount").FirstOrDefault()?.Value),
                            Notes = node.Elements("notes").FirstOrDefault()?.Value,
                            RegistrationNumber = int.Parse(node.Elements("registrationNumber").FirstOrDefault()?.Value),
                            ApplicationDate = new DateTime(int.Parse(node.Elements("applicationDate").Descendants().Where(x => x.Name == "year").FirstOrDefault()?.Value),
                                                     int.Parse(node.Elements("applicationDate").Descendants().Where(x => x.Name == "month").FirstOrDefault()?.Value),
                                                     int.Parse(node.Elements("applicationDate").Descendants().Where(x => x.Name == "day").FirstOrDefault()?.Value)),
                            PublicationDate = new DateTime(int.Parse(node.Elements("publicationDate").Descendants().Where(x => x.Name == "year").FirstOrDefault()?.Value),
                                                     int.Parse(node.Elements("publicationDate").Descendants().Where(x => x.Name == "month").FirstOrDefault()?.Value),
                                                     int.Parse(node.Elements("publicationDate").Descendants().Where(x => x.Name == "day").FirstOrDefault()?.Value)),
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
