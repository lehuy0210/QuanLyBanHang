using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBH.BLL;
using QLBH.Common;
using QLBH.DAL;
using QLBH.DAL.Models;
using System.Data;

namespace QLBH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        OrderBLL bllDonHang = new OrderBLL();

        private readonly QLBH_DBContext _context;
        public OrderController(QLBH_DBContext context) { _context = context; }

        [HttpPost("Create")]
        public IActionResult CreateOrder(string customerId, [FromBody] List<OrderReq> cart)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // 1. Tạo bảng Order (Bảng cha)
                var newOrder = new Order
                {
                    CustomerId = customerId,
                    OrderDate = DateTime.Now,
                    ShipName = "Khách hàng " + customerId // Ví dụ lấy tạm thông tin
                };

                _context.Order.Add(newOrder);
                _context.SaveChanges(); // Để lấy được newOrder.OrderId tự tăng

                // 2. LINQ: Map từ List<CartItem> sang List<OrderDetail>
                // Đây là phần bạn cần nhất: cực nhanh và sạch code
                var orderDetails = cart.Select(c => new OrderDetail
                {
                    OrderId = newOrder.OrderId,
                    ProductId = c.ProductId,
                    UnitPrice = c.UnitPrice,
                    Quantity = (short)c.Quantity, // Ép kiểu về short theo Model của bạn
                    Discount = 0f                 // Kiểu float
                }).ToList();

                // 3. Lưu vào database
                _context.OrderDetail.AddRange(orderDetails);
                _context.SaveChanges();

                transaction.Commit();
                return Ok(new { Message = "Đã lưu vào DB thành công", OrderId = newOrder.OrderId });
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                string loiThatSu = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                return BadRequest(loiThatSu);
            }
        }

        [HttpGet("List")]
        public IActionResult List()
        {
            try
            {
                DataTable dt = bllDonHang.getDonHang();

                // Dùng LINQ để chuyển DataTable thành danh sách đối tượng nặc danh (Anonymous Object)
                //Load về phía client rồi mới select 
                var result = dt.AsEnumerable().Select(row => new {
                    OrderID = row["OrderID"],
                    ContactName = row["ContactName"],
                    UnitPrice = row["UnitPrice"],
                    Quantity = row["Quantity"],
                    TotalPrice = row["TotalPrice"]
                }).ToList();

                return Ok(result);
                // Trả về dạng: [{ "orderID": 1, "contactName": "A"... }, { ... }]
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Detail/{id}")]
        public IActionResult GetOrderDetail(int id)
        {
            try
            {
                // Gọi hàm từ BLL mà bạn vừa viết
                List<OrderReq> result = bllDonHang.layDonHangTheoId(id);

                // Kiểm tra xem đơn hàng có tồn tại/có sản phẩm nào không
                if (result == null || result.Count == 0)
                {
                    return NotFound(new { Message = $"Không tìm thấy chi tiết cho đơn hàng mã {id}" });
                }

                // Trả về danh sách dạng JSON với mã 200 OK
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Nếu có lỗi ở BLL/DAL thì API sẽ quăng mã 400 kèm câu thông báo
                return BadRequest(new { Message = "Lỗi khi lấy dữ liệu: " + ex.Message });
            }
        }
    }
}
