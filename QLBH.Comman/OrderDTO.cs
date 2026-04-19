using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH.DTO
{
    public class OrderDTO
    {

        public int OrderID { get; set; }
        public string? ContactName { get; set; }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        //public string Image { get; set; } 
        public decimal Total => UnitPrice * Quantity;

        public decimal TotalPrice { get; set; }
    }
}
