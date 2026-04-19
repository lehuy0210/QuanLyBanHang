using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH.DTO
{
    public class CustomerDTO
    {
        public string? Id { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public string Phone { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }


        public CustomerDTO()
        {

        }

        public CustomerDTO(string? id, string name, string address, string city, string country, string phone, string username, string password)
        {
            this.Id = id;
            this.Name = name;
            this.Address = address;
            this.City = city;
            this.Country = country;
            this.Phone = phone;
            this.Username = username;
            this.Password = password;
        }
    }
}
