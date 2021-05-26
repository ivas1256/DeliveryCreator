using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCreator.Data
{
    class Receiver
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Fio { get; set; }
        public string Phone { get; set; }

        public override string ToString()
        {
            return string.Join("; ", Name, City, Address, Fio, Phone);
        }

        internal string ToShortString()
        {
            return string.Join("; ", Name, City, Address);
        }
    }
}
