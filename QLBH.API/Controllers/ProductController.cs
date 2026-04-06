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
        private readonly QLBH_DBContext _context;

        public ProductController(ProductBLL bllSanPham, QLBH_DBContext context)
        {
            _bllSanPham = bllSanPham ?? throw new ArgumentNullException(nameof(bllSanPham));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet("GetCategories")]
        public IActionResult GetCategories()
        {
            var data = _context.Categories
                .Select(c => new CategoryReq { Id = c.CategoryId, Name = c.CategoryName })
                .ToList();
            return Ok(data);
        }

        [HttpGet("GetSuppliers")]
        public IActionResult GetSuppliers()
        {
            var data = _context.Suppliers
                .Select(s => new SupplierReq { Id = s.SupplierId, Name = s.CompanyName })
                .ToList();
            return Ok(data);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] ProductReq? sp)
        {
            if (sp == null)
                return BadRequest(ApiResponse.Fail("Dữ liệu sản phẩm không hợp lệ."));

            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(ApiResponse.Fail(errors));
            }

            try
            {
                if (_bllSanPham.themSanPham(sp))
                    return Ok(ApiResponse.Ok("Thêm sản phẩm thành công!"));

                return BadRequest(ApiResponse.Fail("Thêm sản phẩm thất bại."));
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

        [HttpGet("List")]
        public IActionResult List()
        {
            try
            {
                var data = _bllSanPham.getSanPhamDataTable();
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
                var sp = _bllSanPham.laySanPhamTheoId(id);
                if (sp == null)
                    return NotFound(ApiResponse.Fail("Không tìm thấy sản phẩm."));

                return Ok(sp);
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
        public IActionResult Edit([FromBody] ProductReq? sp)
        {
            if (sp == null)
                return BadRequest(ApiResponse.Fail("Dữ liệu sản phẩm không hợp lệ."));

            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(ApiResponse.Fail(errors));
            }

            try
            {
                if (_bllSanPham.suaSanPham(sp))
                    return Ok(ApiResponse.Ok("Cập nhật sản phẩm thành công!"));

                return BadRequest(ApiResponse.Fail("Cập nhật sản phẩm thất bại."));
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
                if (_bllSanPham.xoaSanPham(id))
                    return Ok(ApiResponse.Ok("Xóa sản phẩm thành công!"));

                return BadRequest(ApiResponse.Fail("Sản phẩm không tồn tại hoặc không thể xóa."));
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