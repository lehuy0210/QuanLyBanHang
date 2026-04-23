using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBH.API.Services;
using QLBH.DAL;
using QLBH.DAL.Models;
using QLBH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLBH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly QLBH_DBContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(QLBH_DBContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {
            try
            {
                // 1. TÌM TRONG BẢNG NHÂN VIÊN (ADMIN)
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Username == request.Username);

                if (employee != null)
                {
                    // .Trim() để loại bỏ khoảng trắng rác trong DB
                    if (!BCrypt.Net.BCrypt.Verify(request.Password, employee.Password.Trim()))
                    {
                        return Unauthorized(new { error = new { userMessage = "Mật khẩu không chính xác.", internalMessage = "Sai pass Admin", code = 401 } });
                    }

                    string fullName = employee.FirstName + " " + employee.LastName;
                    string token = _tokenService.CreateToken(employee.EmployeeId.ToString(), employee.Username, fullName, "Admin");

                    return Ok(new
                    {
                        message = "Đăng nhập thành công (Quyền Quản trị)",
                        token = token,
                        userInfo = new
                        {
                            id = employee.EmployeeId.ToString(),
                            username = employee.Username,
                            name = fullName,
                            role = "Admin"
                        }
                    });
                }

                // 2. TÌM TRONG BẢNG KHÁCH HÀNG (USER)
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(u => u.Username == request.Username);

                if (customer != null)
                {
                    if (!BCrypt.Net.BCrypt.Verify(request.Password, customer.Password.Trim()))
                    {
                        return Unauthorized(new { error = new { userMessage = "Mật khẩu không chính xác.", internalMessage = "Sai pass Customer", code = 401 } });
                    }

                    string token = _tokenService.CreateToken(customer.CustomerId.ToString(), customer.Username, customer.ContactName, "User");

                    return Ok(new
                    {
                        message = "Đăng nhập thành công",
                        token = token,
                        userInfo = new
                        {
                            id = customer.CustomerId.ToString(),
                            username = customer.Username,
                            name = customer.ContactName,
                            role = "User"
                        }
                    });
                }

                // 3. KHÔNG TÌM THẤY TÀI KHOẢN
                return NotFound(new { error = new { userMessage = "Tên đăng nhập không tồn tại.", internalMessage = "User not found", code = 404 } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = new { userMessage = "Lỗi hệ thống.", internalMessage = ex.Message, code = 500 } });
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> CreateAccount([FromBody] CustomerDTO request)
        {
            try
            {
                // LOGIC TẠO ID TỰ ĐỘNG (KH001, KH002...)
                string idMoi = "KH001";
                var result = await _context.Customers
                    .Where(c => c.CustomerId.StartsWith("KH"))
                    .OrderByDescending(c => c.CustomerId)
                    .Select(c => c.CustomerId)
                    .FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(result))
                {
                    string idLonNhat = result.Trim();
                    int soHienTai = int.Parse(idLonNhat.Substring(2));
                    idMoi = "KH" + (soHienTai + 1).ToString("D3");
                }

                var newCustomer = new Customer
                {
                    CustomerId = idMoi,
                    CompanyName = "Cá nhân", // Giá trị mặc định chống lỗi Northwind
                    Username = request.Username,
                    ContactName = request.Name,
                    Address = request.Address,
                    City = request.City,
                    Country = request.Country,
                    Phone = request.Phone,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password) // Băm mật khẩu chuẩn BCrypt
                };

                _context.Customers.Add(newCustomer);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Đăng ký thành công!" });
            }
            catch (Exception ex)
            {
                string rootError = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new { error = new { userMessage = "Đăng ký thất bại.", internalMessage = rootError, code = 500 } });
            }
        }
    }
}