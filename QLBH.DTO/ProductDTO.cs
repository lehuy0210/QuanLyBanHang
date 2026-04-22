using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Quantity { get; set; }

        public int CateId { get; set; }
        public int SupId { get; set; }

        public int UnitsInStock { get; set; }

        public IEnumerable<CategoryDTO>? Categories { get; set; }
        public IEnumerable<SupplierDTO>? Suppliers { get; set; }

        public ProductDTO()
        {

        }

        public ProductDTO(int id, string name, decimal price, string quantity, int cateid, int supid, int uis)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.Quantity = quantity;
            this.CateId = cateid;
            this.SupId = supid;
            this.UnitsInStock = uis;
        }

    }
}
