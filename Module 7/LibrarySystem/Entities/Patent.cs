using System;
using System.Collections.Generic;

namespace LibrarySystem
{
    public class Patent: ICatalogEntity
    {
        public string Name { get; set; }
        public List<string> Inventors { get; set; }
        public string City { get; set; }
        public int RegistrationNumber { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime PublicationDate { get; set; }
        public int PageCount { get; set; }
        public string Notes { get; set; }

        public Patent(string name, List <string> inventors, string city, int registrationNumber, int dayApp, int monthApp, int yearApp, int dayPub, int monthPub, int yearPub, int pageCount, string notes)
        {
            if (string.IsNullOrWhiteSpace(name)||(registrationNumber == 0))
            {
                throw new ArgumentOutOfRangeException();
            }

            Name = name;
            Inventors = new List<string>();
            foreach (var inventor in inventors)
            {
                Inventors.Add(inventor);
            }
            City = city;
            RegistrationNumber = registrationNumber;
            try
            {
                ApplicationDate = new DateTime(yearApp, monthApp, dayApp);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException("ApplicationDate is out of range");
            }
            try
            {
                PublicationDate = new DateTime(yearPub, monthPub, dayPub);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentOutOfRangeException("PublicationDate is out of range");
            }
            PageCount = pageCount;
            Notes = notes;
            }

        public Patent ()
        { }

    }
}
