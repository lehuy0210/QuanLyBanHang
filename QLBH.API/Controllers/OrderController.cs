using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBH.Common;
using QLBH.DAL;
using QLBH.DAL.Models;

namespace QLBH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
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

                _context.Orders.Add(newOrder);
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
                _context.OrderDetails.AddRange(orderDetails);
                _context.SaveChanges();

                transaction.Commit();
                return Ok(new { Message = "Đã lưu vào DB thành công", OrderId = newOrder.OrderId });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return BadRequest(ex.Message);
            }
        }
    }
}
