using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBH.BLL;
using QLBH.DAL;
using QLBH.DAL.Models;
using QLBH.DTO;
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
        public IActionResult CreateOrder(string customerId, [FromBody] List<OrderDTO> cart)
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

                var orderDetails = new List<OrderDetail>();

                foreach (var c in cart)
                {
               
                    orderDetails.Add(new OrderDetail
                    {
                        OrderId = newOrder.OrderId,
                        ProductId = c.ProductId,
                        UnitPrice = c.UnitPrice,
                        Quantity = (short)c.Quantity,
                        Discount = 0f
                    });

                }

                _context.OrderDetail.AddRange(orderDetails);

                _context.SaveChanges(); //KHI GỌI LỆNH NÀY, TRIGGER trg_CheckStock SẼ NHẢY VÀO LÀM VIỆC!

                transaction.Commit();
                return Ok(new { Message = "Đã lưu vào DB thành công", OrderId = newOrder.OrderId });
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                string loiThatSu = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                return StatusCode(500, new
                {
                    error = new
                    {
                        userMessage = "Hệ thống gặp sự cố trong quá trình tạo đơn hàng, vui lòng thử lại sau",
                        internalMessage = loiThatSu,
                        code = 50
                    }
                });
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
                    Quantity = row["Quantity"],
                    TotalPrice = row["TotalPrice"]
                }).ToList();

                return Ok(result);
  
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = new
                    {
                        userMessage = "Hệ thống gặp sự cố khi lấy danh sách đơn hàng",
                        internalMessage = ex.Message,
                        code = 50
                    }
                });
            }
        }

        [HttpGet("Detail/{id}")]
        public IActionResult GetOrderDetail(int id)
        {
            try
            {
                // Gọi hàm từ BLL mà bạn vừa viết
                List<OrderDTO> result = bllDonHang.layDonHangTheoId(id);

                // Kiểm tra xem đơn hàng có tồn tại/có sản phẩm nào không
                if (result == null || result.Count == 0)
                {
                    return NotFound(new
                    {
                        error = new
                        {
                            userMessage = "Đơn hàng không tồn tại",
                            internalMessage = $"Chi tiết đơn hàng {id} không có trong database",
                            code = 34
                        }
                    });
                }

                // Trả về danh sách dạng JSON với mã 200 OK
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = new
                    {
                        userMessage = "Hệ thống gặp sự cố, vui lòng thử lại sau",
                        internalMessage = ex.Message,
                        code = 50
                    }
                });
            }
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                bool isSuccess = bllDonHang.XoaDonHang(id);

                if (isSuccess)
                {
                    return Ok(new { Message = "Đã xóa đơn hàng và cập nhật lại kho thành công!" });
                }
                else
                {
                    return BadRequest(new
                    {
                        error = new
                        {
                            userMessage = "Không thể xóa đơn hàng lúc này",
                            internalMessage = "Không thể xóa đơn hàng trong database",
                            code = 40
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Đơn hàng không tồn tại"))
                {
                    return NotFound(new
                    {
                        error = new
                        {
                            userMessage = "Đơn hàng không tồn tại",
                            internalMessage = "Không tìm thấy đơn hàng trong database để xóa",
                            code = 34
                        }
                    });
                }
                return StatusCode(500, new
                {
                    error = new
                    {
                        userMessage = "Hệ thống gặp sự cố, vui lòng thử lại sau",
                        internalMessage = ex.Message,
                        code = 50
                    }
                });
            }
        }
    }
}
