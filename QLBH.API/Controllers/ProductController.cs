using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBH.BLL;
using QLBH.DAL;
using QLBH.DTO;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace QLBH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductBLL bllSanPham = new ProductBLL();
        private readonly QLBH_DBContext _context;
        public ProductController(QLBH_DBContext context)
        {
            _context = context;
        }

        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            try
            {
                var data = _context.Categories.Select(c => new CategoryDTO { Id = c.CategoryId, Name = c.CategoryName }).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = new
                    {
                        userMessage = ex.Message,
                        internalMessage = ex.InnerException?.Message,
                        code = 50
                    }
                });
            }
        }

        [HttpGet("suppliers")]
        public IActionResult GetSuppliers()
        {
            try
            {
                var data = _context.Suppliers.Select(s => new SupplierDTO { Id = s.SupplierId, Name = s.CompanyName }).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = new
                    {
                        userMessage = ex.Message,
                        internalMessage = ex.InnerException?.Message,
                        code = 50
                    }
                });
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProductDTO sp)
        {
            try
            {
                if (bllSanPham.themSanPham(sp))
                {
                    return StatusCode(201, new { success = true, message = "Thêm sản phẩm thành công!" });
                }
                else
                {
                    return BadRequest(new
                    {
                        error = new
                        {
                            userMessage = "Không thể tạo mới dữ liệu sản phẩm",
                            internalMessage = "Failed to insert product into the database",
                            code = 40
                        }
                    });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new // Đã đổi thành 500 (hoặc bạn có thể để 400 tùy logic)
                {
                    error = new
                    {
                        userMessage = ex.Message,
                        internalMessage = ex.InnerException?.Message,
                        code = 50
                    }
                });
            }

        }
        [HttpGet]
        public IActionResult List()
        {
            try
            {
                return Ok(bllSanPham.getSanPham());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = new
                    {
                        userMessage = ex.Message,
                        internalMessage = ex.InnerException?.Message,
                        code = 50
                    }
                });
            }
        }
        [HttpGet("{id:int}")]

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
                    return NotFound(new
                    {
                        error = new
                        {
                            userMessage = "Sản phẩm không tồn tại",
                            internalMessage = "Không tìm thấy sản phẩm trong database",
                            code = 34
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = new
                    {
                        userMessage = ex.Message,
                        internalMessage = ex.InnerException?.Message,
                        code = 50
                    }
                });
            }
        }
        [HttpPut("{id:int}")]
        public IActionResult Edit([FromBody] ProductDTO sp)
        {
            try
            {
                if (bllSanPham.suaSanPham(sp))
                {
                    return Ok(new { success = true, message = "Cập nhật sản phẩm thành công!" });
                }
                else
                {
                    return BadRequest(new
                    {
                        error = new
                        {
                            userMessage = "Không thể cập nhật thôngত্তি sản phẩm",
                            internalMessage = "Không cập nhật được sản phẩm trong database",
                            code = 40
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = new
                    {
                        userMessage = ex.Message,
                        internalMessage = ex.InnerException?.Message,
                        code = 50
                    }
                });
            }
        }

        [HttpDelete("{id:int}")]
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
                    return NotFound(new
                    {
                        error = new
                        {
                            userMessage = "Sản phẩm không tồn tại",
                            internalMessage = "Không tìm thấy sản phẩm trong database để xóa",
                            code = 34
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = new
                    {
                        userMessage = ex.Message,
                        internalMessage = ex.InnerException?.Message,
                        code = 50
                    }
                });
            }
        }

        [HttpGet("search/{tukhoa}")]
        public IActionResult Search(string tukhoa)
        {
            try
            {
                var dtResult = bllSanPham.getSanPhamTimKiem(tukhoa);

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    return Ok(dtResult);
                }
                else
                {
                    return Ok(new List<object>());
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = new
                    {
                        userMessage = ex.Message,
                        internalMessage = ex.InnerException?.Message,
                        code = 50
                    }
                });
            }
        }
    }
}