using Microsoft.AspNetCore.Mvc;
using QLBH.BLL;
using QLBH.DTO;
using QLBH.DAL;

namespace QLBH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly CustomerBLL bllKhachHang = new CustomerBLL();

        [HttpGet]
        public IActionResult List()
        {
            return Ok(bllKhachHang.getKhachHang());
        }

        [HttpPost]

        public IActionResult Create([FromBody] CustomerDTO kh)
        {
            try
            {
                if (bllKhachHang.themKhachHang(kh))
                {
                    return Ok(new { success = true, message = "Thêm khách hàng thành công!" });
                }
                else
                {
                    return BadRequest(new
                    {
                        error = new
                        {
                            userMessage = "Không thể tạo mới dữ liệu khách hàng",
                            internalMessage = "Thất bại khi thêm khách hàng vào database",
                            code = 40 
                        }
                    });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(400, new
                {
                    error = new
                    {
                        userMessage = ex.Message,
                        code = 40
                    }
                });
            }
        }

        [HttpGet("{id}")]
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
                    return NotFound(new
                    {
                        error = new
                        {
                            userMessage = "Không tìm thấy khách hàng này",
                            internalMessage = "Không tìm thấy khách hàng trong database",
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
                        userMessage = "Hệ thống gặp sự cố, vui lòng thử lại sau",
                        internalMessage = ex.Message,
                        code = 50
                    }
                });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                if(bllKhachHang.xoaKhachHang(id))
                {
                    return Ok(true);    
                }
                else
                {
                    return NotFound(new
                    {
                        error = new
                        {
                            userMessage = "Không tìm thấy khách hàng này",
                            internalMessage = "Không tìm thấy khách hàng trong database",
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
                        userMessage = "Hệ thống gặp sự cố, vui lòng thử lại sau",
                        internalMessage = ex.Message,
                        code = 50
                    }
                });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Edit(string id, [FromBody] CustomerDTO kh)
        {
            if (id != kh.Id)
            {
                return BadRequest(new { message = "ID trên URL không khớp với dữ liệu gửi lên!" });
            }

            try
            {
                if(bllKhachHang.suaKhachHang(kh))
                {
                    return Ok(new { success = true, message = "Cập nhật khách hàng thành công!" });
                }
                else
                {
                    return BadRequest(new
                    {
                        error = new
                        {
                            userMessage = "Không thể cập nhật thông tin khách hàng",
                            internalMessage = "Không thể cập nhật khách hàng trong database",
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
                        userMessage = "Hệ thống gặp sự cố, vui lòng thử lại sau",
                        internalMessage = ex.Message,
                        code = 50
                    }
                });
            }
        }
    }
}
