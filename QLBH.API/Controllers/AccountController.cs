using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBH.DAL;
using QLBH.DTO;
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
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            try
            {
                // 1. TÌM TRONG BẢNG NHÂN VIÊN (ADMIN) TRƯỚC
                // (Lưu ý: Bạn hãy sửa lại tên trường "Username", "Password", "EmployeeId", "EmployeeName" 
                // cho khớp với thiết kế Entity Employee của bạn nhé)
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Username == request.Username);

                if (employee != null)
                {
                    if (employee.Password != request.Password)
                    {
                        return Unauthorized("Mật khẩu không chính xác.");
                    }

                    // Trả về thông tin Nhân viên + Gắn mác Role: "Admin"
                    return Ok(new
                    {
                        message = "Đăng nhập thành công (Quyền Quản trị)",
                        userInfo = new
                        {
                            id = employee.EmployeeId.ToString(), // Chuyển sang chuỗi cho đồng nhất
                            username = employee.Username,
                            name = employee.FirstName + " "+ employee.LastName, // Thay bằng tên cột Tên Nhân Viên của bạn
                            role = "Admin" // <-- QUAN TRỌNG NHẤT LÀ ĐÂY
                        }
                    });
                }

                // 2. NẾU KHÔNG PHẢI NHÂN VIÊN, TÌM TIẾP TRONG BẢNG KHÁCH HÀNG (USER)
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(u => u.Username == request.Username);

                if (customer != null)
                {
                    if (customer.Password != request.Password)
                    {
                        return Unauthorized("Mật khẩu không chính xác.");
                    }

                    // Trả về thông tin Khách hàng + Gắn mác Role: "User"
                    return Ok(new
                    {
                        message = "Đăng nhập thành công",
                        userInfo = new
                        {
                            id = customer.CustomerId.ToString(),
                            username = customer.Username,
                            name = customer.ContactName,
                            role = "User" // <-- GẮN MÁC USER
                        }
                    });
                }

                // 3. KHÔNG TÌM THẤY Ở CẢ 2 BẢNG
                return BadRequest("Tên đăng nhập không tồn tại trong hệ thống.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi server: " + ex.Message);
            }
        }
    }
}
