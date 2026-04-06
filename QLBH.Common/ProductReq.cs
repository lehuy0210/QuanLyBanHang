using System.ComponentModel.DataAnnotations;

namespace QLBH.Common
{
    public class ProductReq
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        [MaxLength(40, ErrorMessage = "Tên sản phẩm tối đa 40 ký tự.")]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải >= 0.")]
        public decimal Price { get; set; }

        [MaxLength(20, ErrorMessage = "Quy cách tối đa 20 ký tự.")]
        public string Quantity { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn loại sản phẩm.")]
        public int CateId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn nhà cung cấp.")]
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
