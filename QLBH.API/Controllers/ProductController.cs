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
        private readonly ProductBLL bllSanPham = new ProductBLL();

        [HttpPost("Create")]
        public IActionResult Create([FromBody] ProductReq sp)
        {
            try
            {
                if (bllSanPham.themSanPham(sp))
                {
                    return Ok(new { success = true, message = "Thêm sản phẩm thành công!" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Thêm sản phẩm thất bại." });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(404, new { success = false, message = "Lỗi server: " + ex.Message });
            }

        }
        [HttpPost("List")]
        public IActionResult List()
        {
            ProductDAL dalSanPham = new ProductDAL();

            return Ok(dalSanPham.getSanPham());
        }
        [HttpGet("GetByID")]

        public IActionResult GetById(int id)
        {
            try
            {
                var sp = bllSanPham.laySanPhamTheoId(id);

                if (sp != null)
                {
                    return Ok(sp);
                }
                else
                {
                    return NotFound(new { success = false, message = "Không tìm thấy sản phẩm." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(404, new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }
        [HttpPost("Edit")]
        public IActionResult Edit([FromBody] ProductReq sp)
        {
            try
            {
                if (bllSanPham.suaSanPham(sp))
                {
                    return Ok(new { success = true, message = "Cập nhật sản phẩm thành công!" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Cập nhật sản phẩm thất bại." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(404, new { success = false, message = "Lỗi server: " + ex.Message });
            }
        }

        [HttpPost("Delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (bllSanPham.xoaSanPham(id))
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest("Sản phẩm không tồn tại hoặc không thể xóa.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
        }
    }
}