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

        [HttpGet]
        public IActionResult List()
        {
            return Ok(bllNhanVien.getNhanVien());
        }

        [HttpPost]
        public IActionResult Create([FromBody] EmployeeDTO nv)
        {
            try
            {
                if (bllNhanVien.themNhanVien(nv))
                {
                    return Ok(new { success = true, message = "Thêm nhân viên thành công!" });
                }
                else
                {
                    return BadRequest(new
                    {
                        error = new
                        {
                            userMessage = "Không thể tạo mới dữ liệu nhân viên",
                            internalMessage = "Thất bại khi thêm nhân viên vào database",
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

        [HttpGet("{id}", Name = "GetEmployeeById")]
        public IActionResult GetById(int id)
        {
            try
            {
                var nv = bllNhanVien.layNVTheoID(id);

                if (nv != null)
                {
                    return Ok(nv);
                }
                else
                {
                    return NotFound(new
                    {
                        error = new
                        {
                            userMessage = "Không tìm thấy nhân viên này",
                            internalMessage = "Không tìm thấy nhân viên trong database",
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

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                if (bllNhanVien.xoaNhanVien(id))
                {
                    return Ok(true);
                }
                else
                {
                    return NotFound(new
                    {
                        error = new
                        {
                            userMessage = "Không tìm thấy nhân viên này",
                            internalMessage = "Không tìm thấy nhân viên trong database để xóa",
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

        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] EmployeeDTO nv)
        {
            try
            {
                if (bllNhanVien.suaNhanVien(nv))
                {
                    return Ok(new { success = true, message = "Cập nhật nhân viên thành công!" });
                }
                else
                {
                    return BadRequest(new
                    {
                        error = new
                        {
                            userMessage = "Không thể cập nhật thông tin nhân viên",
                            internalMessage = "Không thể cập nhật nhân viên trong database",
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
    }
}