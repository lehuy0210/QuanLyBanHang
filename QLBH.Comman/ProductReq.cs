using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH.Common
{
    public class ProductReq
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Quantity { get; set; } = string.Empty;

        public int CateId { get; set; }
        public int SupId { get; set; }

        public IEnumerable<CategoryReq>? Categories { get; set; }
        public IEnumerable<SupplierReq>? Suppliers { get; set; }

        public ProductReq()
        {
        }

        public ProductReq(int id, string name, decimal price, string quantity, int cateid, int supid)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
            CateId = cateid;
            SupId = supid;
        }
    }
}
