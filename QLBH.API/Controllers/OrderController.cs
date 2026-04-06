using Microsoft.AspNetCore.Mvc;
using QLBH.BLL;
using QLBH.Common;
using QLBH.DAL;

namespace QLBH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderBLL _bllOrder;
        private readonly QLBH_DBContext _context;

        public OrderController(OrderBLL bllOrder, QLBH_DBContext context)
        {
            _bllOrder = bllOrder ?? throw new ArgumentNullException(nameof(bllOrder));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet("List")]
        public IActionResult List()
        {
            try
            {
                var data = _bllOrder.getOrders();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Fail("Lỗi server: " + ex.Message));
            }
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            try
            {
                var order = _bllOrder.getOrderById(id);
                if (order == null)
                    return NotFound(ApiResponse.Fail("Không tìm thấy đơn hàng."));

                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Fail("Lỗi server: " + ex.Message));
            }
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] OrderReq? order)
        {
            if (order == null)
                return BadRequest(ApiResponse.Fail("Dữ liệu đơn hàng không hợp lệ."));

            try
            {
                var orderId = _bllOrder.themOrder(order);
                return Ok(ApiResponse<int>.Ok(orderId, "Tạo đơn hàng thành công!"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Fail("Lỗi server: " + ex.Message));
            }
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (_bllOrder.xoaOrder(id))
                    return Ok(ApiResponse.Ok("Xóa đơn hàng thành công!"));

                return BadRequest(ApiResponse.Fail("Đơn hàng không tồn tại hoặc không thể xóa."));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Fail("Lỗi server: " + ex.Message));
            }
        }

        [HttpGet("GetProducts")]
        public IActionResult GetProducts()
        {
            var data = _context.Products
                .Where(p => !p.Discontinued)
                .Select(p => new { p.ProductId, p.ProductName, p.UnitPrice })
                .ToList();
            return Ok(data);
        }

        [HttpGet("GetCustomers")]
        public IActionResult GetCustomers()
        {
            var data = _context.Customers
                .Select(c => new { c.CustomerId, c.ContactName, c.CompanyName })
                .ToList();
            return Ok(data);
        }
    }
}
