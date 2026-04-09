using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBH.Common;
using QLBH.DAL;
using System;

namespace QLBH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly QLBH_DBContext _context;
        public AccountController(QLBH_DBContext context)
        {
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginReq request)
        {
            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(u => u.Username == request.Username);

                if (customer == null)
                {
                    return BadRequest("Tên đăng nhập không tồn tại.");
                }

                if (customer.Password != request.Password)
                {
                    return Unauthorized("Mật khẩu không chính xác.");
                }

                return Ok(new
                {
                    message = "Đăng nhập thành công",

                    userInfo = new
                    {
                        Id = customer.CustomerId,
                        Username = customer.Username,
                        Name = customer.ContactName
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi server: " + ex.Message);
            }
        }
    }
}
