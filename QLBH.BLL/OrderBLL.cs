using QLBH.Common;
using QLBH.DAL;

namespace QLBH.BLL
{
    public class OrderBLL
    {
        private readonly OrderDAL _dalOrder;

        public OrderBLL(OrderDAL dalOrder)
        {
            _dalOrder = dalOrder ?? throw new ArgumentNullException(nameof(dalOrder));
        }

        public List<OrderReq> getOrders()
        {
            return _dalOrder.getOrders();
        }

        public OrderReq? getOrderById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Mã đơn hàng không hợp lệ.");
            return _dalOrder.getOrderById(id);
        }

        public int themOrder(OrderReq order)
        {
            ValidateOrder(order);
            return _dalOrder.themOrder(order);
        }

        public bool xoaOrder(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Mã đơn hàng không hợp lệ.");
            return _dalOrder.xoaOrder(id);
        }

        private void ValidateOrder(OrderReq order)
        {
            if (string.IsNullOrWhiteSpace(order.CustomerId))
                throw new ArgumentException("Vui lòng chọn khách hàng.");

            if (order.OrderDetails == null || order.OrderDetails.Count == 0)
                throw new ArgumentException("Đơn hàng phải có ít nhất 1 sản phẩm.");

            foreach (var detail in order.OrderDetails)
            {
                if (detail.ProductId <= 0)
                    throw new ArgumentException("Sản phẩm không hợp lệ.");
                if (detail.Quantity <= 0)
                    throw new ArgumentException("Số lượng phải >= 1.");
                if (detail.UnitPrice < 0)
                    throw new ArgumentException("Đơn giá phải >= 0.");
            }
        }
    }
}
