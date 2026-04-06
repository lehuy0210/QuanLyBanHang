using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH.Common
{
    public class CustomerReq
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public string Phone { get; set; }


        public CustomerReq()
        {

        }

        public CustomerReq(int id, string name, string address, string city, string country, string phone)
        {
            this.Id = id;
            this.Name = name;
            this.Address = address;
            this.City = city;
            this.Country = country;
            this.Phone = phone;
        }
    }
}
