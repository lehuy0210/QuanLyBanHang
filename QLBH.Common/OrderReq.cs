using System.ComponentModel.DataAnnotations;

namespace QLBH.Common
{
    public class OrderReq
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn khách hàng.")]
        public string CustomerId { get; set; } = string.Empty;

        public int? EmployeeId { get; set; }

        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }

        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }

        [MaxLength(40)]
        public string? ShipName { get; set; }
        [MaxLength(60)]
        public string? ShipAddress { get; set; }
        [MaxLength(15)]
        public string? ShipCity { get; set; }
        [MaxLength(15)]
        public string? ShipRegion { get; set; }
        [MaxLength(10)]
        public string? ShipPostalCode { get; set; }
        [MaxLength(15)]
        public string? ShipCountry { get; set; }

        // Navigation helpers for views
        public string? CustomerName { get; set; }
        public string? EmployeeName { get; set; }

        public List<OrderDetailReq> OrderDetails { get; set; } = new();

        // Dropdown data for views
        public IEnumerable<CustomerReq>? Customers { get; set; }
    }

    public class OrderDetailReq
    {
        private decimal _unitPrice;
        private short _quantity = 1;
        private float _discount;
        private decimal? _extendedPriceCache;

        public int OrderId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn sản phẩm.")]
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Đơn giá phải > 0.")]
        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                if (_unitPrice == value)
                {
                    return;
                }

                _unitPrice = value;
                InvalidateExtendedPrice();
            }
        }

        [Range(1, short.MaxValue, ErrorMessage = "Số lượng phải >= 1.")]
        public short Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity == value)
                {
                    return;
                }

                _quantity = value;
                InvalidateExtendedPrice();
            }
        }

        [Range(0, 1, ErrorMessage = "Giảm giá phải từ 0 đến 1.")]
        public float Discount
        {
            get => _discount;
            set
            {
                if (_discount.Equals(value))
                {
                    return;
                }

                _discount = value;
                InvalidateExtendedPrice();
            }
        }

        public decimal ExtendedPrice => _extendedPriceCache ??= _unitPrice * _quantity * (1 - (decimal)_discount);

        private void InvalidateExtendedPrice()
        {
            _extendedPriceCache = null;
        }
    }
}
