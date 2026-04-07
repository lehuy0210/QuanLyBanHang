using Microsoft.AspNetCore.Mvc;
using QLBH.BLL;
using QLBH.Common;
using QLBH.DAL;

namespace QLBH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly CustomerBLL bllKhachHang = new CustomerBLL();

        [HttpGet("List")]
        public IActionResult List()
        {
            return Ok(bllKhachHang.getKhachHang());
        }

        [HttpPost("Create")]

        public IActionResult Create([FromBody] CustomerReq kh)
        {
            try
            {
                if (bllKhachHang.themKhachHang(kh))
                {
                    return Ok(new { success = true, message = "Thêm khách hàng thành công!" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Thêm khách hàng thất bại." });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(400, new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpGet("GetByID")]
        public IActionResult GetById(string id)
        {
            try
            {
                var kh = bllKhachHang.layKHTheoID(id);

                if (kh != null)
                {
                    return Ok(kh);
                }
                else
                {
                    return NotFound(new { success = false, message = "Không tìm thấy khách hàng." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] string id)
        {
            try
            {
                if(bllKhachHang.xoaKhachHang(id))
                {
                    return Ok(true);    
                }
                else
                {
                    return BadRequest("Khách hàng không tồn tại hoặc không thể xóa.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpPut("Edit")]
        public IActionResult Edit([FromBody] CustomerReq kh)
        {
            try
            {
                if(bllKhachHang.suaKhachHang(kh))
                {
                    return Ok(new { success = true, message = "Cập nhật khách hàng thành công!" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Cập nhật khách hàng thất bại." });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(400, new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }
    }
}
