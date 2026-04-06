using Microsoft.AspNetCore.Mvc;
using QLBH.BLL;
using QLBH.Common;

namespace QLBH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerBLL _bllKhachHang;

        public CustomerController(CustomerBLL bllKhachHang)
        {
            _bllKhachHang = bllKhachHang ?? throw new ArgumentNullException(nameof(bllKhachHang));
        }

        [HttpGet("List")]
        public IActionResult List()
        {
            try
            {
                var data = _bllKhachHang.getKhachHang();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.Fail("Lỗi server: " + ex.Message));
            }
        }

        [HttpGet("GetById")]
        public IActionResult GetById(string id)
        {
            try
            {
                var kh = _bllKhachHang.layKhachHangTheoId(id);
                if (kh == null)
                    return NotFound(ApiResponse.Fail("Không tìm thấy khách hàng."));

                return Ok(kh);
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
        public IActionResult Create([FromBody] CustomerReq? kh)
        {
            if (kh == null)
                return BadRequest(ApiResponse.Fail("Dữ liệu khách hàng không hợp lệ."));

            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(ApiResponse.Fail(errors));
            }

            try
            {
                if (_bllKhachHang.themKhachHang(kh))
                    return Ok(ApiResponse.Ok("Thêm khách hàng thành công!"));

                return BadRequest(ApiResponse.Fail("Thêm khách hàng thất bại."));
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

        [HttpPut("Edit")]
        public IActionResult Edit([FromBody] CustomerReq? kh)
        {
            if (kh == null)
                return BadRequest(ApiResponse.Fail("Dữ liệu khách hàng không hợp lệ."));

            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(ApiResponse.Fail(errors));
            }

            try
            {
                if (_bllKhachHang.suaKhachHang(kh))
                    return Ok(ApiResponse.Ok("Cập nhật khách hàng thành công!"));

                return BadRequest(ApiResponse.Fail("Cập nhật khách hàng thất bại."));
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
        public IActionResult Delete(string id)
        {
            try
            {
                if (_bllKhachHang.xoaKhachHang(id))
                    return Ok(ApiResponse.Ok("Xóa khách hàng thành công!"));

                return BadRequest(ApiResponse.Fail("Khách hàng không tồn tại hoặc không thể xóa."));
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
    }
}
