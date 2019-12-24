using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem
{
    public class Patent
    {
        public string Name { get; set; }
        public List<string> Inventors { get; set; }
        public string City { get; set; }
        public int RegistrationNumber { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime PublicationDate { get; set; }
        public int PageCount { get; set; }
        public string Notes { get; set; }

        public Patent(string name, List <string> inventors, string city, int registrationNumber, DateTime applicationDate, DateTime publicationDate, int pageCount, string notes)
        {
            if (string.IsNullOrWhiteSpace(name)||(registrationNumber == 0))
            {
                throw new ArgumentOutOfRangeException();
            }

            Name = name;
            foreach (var inventor in inventors)
            {
                Inventors.Add(inventor);
            }
            City = city;
            RegistrationNumber = registrationNumber;
            ApplicationDate = applicationDate;
            PublicationDate = publicationDate;
            PageCount = pageCount;
            Notes = notes;
        }

        public Patent ()
        { }

    }
}
