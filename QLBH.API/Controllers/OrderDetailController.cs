using Microsoft.AspNetCore.Mvc;
using QLBH.DTO;
using QLBH.BLL;

namespace QLBH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        OrderBLL bllDonHang = new OrderBLL();

        [HttpGet("{idKH}")]
        public IActionResult GetOrderDetailByCustomer(string idKH)
        {
            try
            {
                // Gọi hàm từ BLL mà bạn vừa viết
                List<OrderDTO> result = bllDonHang.layDonHangTheoKH(idKH);

                // Kiểm tra xem đơn hàng có tồn tại/có sản phẩm nào không
                if (result == null || result.Count == 0)
                {
                    return NotFound(new
                    {
                        error = new
                        {
                            userMessage = "Đơn hàng không tồn tại",
                            internalMessage = $"Chi tiết đơn hàng của {idKH} không có trong database",
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
    }
}
