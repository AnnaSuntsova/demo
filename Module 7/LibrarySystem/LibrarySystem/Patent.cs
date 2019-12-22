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
        public string[] Inventors { get; set; }
        public string City { get; set; }
        public int RegistrationNumber { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime PublicationDate { get; set; }
        public int PageCount { get; set; }
        public string Notes { get; set; }
    }
}
