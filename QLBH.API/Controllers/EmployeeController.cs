using Microsoft.AspNetCore.Mvc;
using QLBH.BLL;
using QLBH.DTO;
using System;

namespace QLBH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly EmployeeBLL bllNhanVien = new EmployeeBLL();

        [HttpGet("List")]
        public IActionResult List()
        {
            return Ok(bllNhanVien.getNhanVien());
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] EmployeeDTO nv)
        {
            try
            {
                if (bllNhanVien.themKhachHang(nv))
                {
                    return Ok(new { success = true, message = "Thêm nhân viên thành công!" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Thêm nhân viên thất bại." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpGet("GetByID")]
        public IActionResult GetById(int id)
        {
            try
            {
                var nv = bllNhanVien.layKHTheoID(id);

                if (nv != null)
                {
                    return Ok(nv);
                }
                else
                {
                    return NotFound(new { success = false, message = "Không tìm thấy nhân viên." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] int id)
        {
            try
            {
                if (bllNhanVien.xoaKhachHang(id))
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest("Nhân viên không tồn tại hoặc không thể xóa.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpPut("Edit")]
        public IActionResult Edit([FromBody] EmployeeDTO nv)
        {
            try
            {
                if (bllNhanVien.suaKhachHang(nv))
                {
                    return Ok(new { success = true, message = "Cập nhật nhân viên thành công!" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Cập nhật nhân viên thất bại." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }
    }
}