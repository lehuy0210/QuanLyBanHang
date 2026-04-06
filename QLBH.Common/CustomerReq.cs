using System.ComponentModel.DataAnnotations;

namespace QLBH.Common
{
    public class CustomerReq
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên liên hệ không được để trống.")]
        [MaxLength(30, ErrorMessage = "Tên liên hệ tối đa 30 ký tự.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(60, ErrorMessage = "Địa chỉ tối đa 60 ký tự.")]
        public string Address { get; set; } = string.Empty;

        [MaxLength(15, ErrorMessage = "Thành phố tối đa 15 ký tự.")]
        public string City { get; set; } = string.Empty;

        [MaxLength(15, ErrorMessage = "Quốc gia tối đa 15 ký tự.")]
        public string Country { get; set; } = string.Empty;

        [MaxLength(24, ErrorMessage = "Số điện thoại tối đa 24 ký tự.")]
        public string Phone { get; set; } = string.Empty;

        public CustomerReq()
        {
        }

        public CustomerReq(string id, string name, string address, string city, string country, string phone)
        {
            Id = id;
            Name = name;
            Address = address;
            City = city;
            Country = country;
            Phone = phone;
        }
    }
}
