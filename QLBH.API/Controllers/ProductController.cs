using System;
using Microsoft.AspNetCore.Mvc;
using QLBH.BLL;
using QLBH.Common;
using QLBH.DAL;

namespace QLBH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductBLL _bllSanPham;

        public ProductController(ProductBLL bllSanPham)
        {
            _bllSanPham = bllSanPham;
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] ProductReq? sp)
        {
            if (sp == null)
            {
                return BadRequest(new { success = false, message = "Dữ liệu sản phẩm không hợp lệ." });
            }

            try
            {
                if (_bllSanPham.themSanPham(sp))
                {
                    return Ok(new { success = true, message = "Thêm sản phẩm thành công!" });
                }

                return BadRequest(new { success = false, message = "Thêm sản phẩm thất bại." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpGet("List")]
        public IActionResult List()
        {
            return Ok(_bllSanPham.getSanPham());
        }

        [HttpGet("GetByID")]
        public IActionResult GetById(int id)
        {
            try
            {
                var sp = _bllSanPham.laySanPhamTheoId(id);

                if (sp == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy sản phẩm." });
                }

                return Ok(sp);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpPost("Edit")]
        public IActionResult Edit([FromBody] ProductReq? sp)
        {
            if (sp == null)
            {
                return BadRequest(new { success = false, message = "Dữ liệu sản phẩm không hợp lệ." });
            }

            try
            {
                if (_bllSanPham.suaSanPham(sp))
                {
                    return Ok(new { success = true, message = "Cập nhật sản phẩm thành công!" });
                }

                return BadRequest(new { success = false, message = "Cập nhật sản phẩm thất bại." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (_bllSanPham.xoaSanPham(id))
                {
                    return Ok(true);
                }

                return BadRequest("Sản phẩm không tồn tại hoặc không thể xóa.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }
    }
}