using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLBH.DTO;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace QLBH.Web.Controllers
{
    [AllowAnonymous] // QUAN TRỌNG: Cho phép truy cập không cần đăng nhập
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        public AccountController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5003/");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(CustomerDTO request, string Matkhaunhaplai)
        {
            if (request.Password != Matkhaunhaplai)
            {
                ViewBag.ThongBaoLoi = "Mật khẩu nhập lại không khớp!";
                return View(request);
            }
            //request.Address = request.Address ?? "";
            request.City = request.City ?? "";
            request.Country = request.Country ?? "";
            try
            {
                var jsonRequest = JsonSerializer.Serialize(request);
                /* JsonSerializer Cung cấp chức năng chuyển đổi các đối tượng hoặc kiểu giá trị thành định dạng JSON, 
                và chuyển đổi ngược JSON lại thành các đối tượng hoặc kiểu giá trị */
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("api/Customer", content);
                /*Post Async là gửi một yêu cầu POST đến URI đã được chỉ định dưới dạng một thao tác bất đồng bộ.*/
                /*Tham số thứ 1 là URL vào BaseAddress*/
                /*Tham số thứ 2 là StringContent*/

                if (response.IsSuccessStatusCode) //StatusCode nằm trong khoảng 200-299
                {
                    TempData["ThongBao"] = "Đăng ký thành công!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // 1. Đọc nội dung lỗi từ API (chuỗi JSON)
                    string errorResult = await response.Content.ReadAsStringAsync();

                    // 2. Sử dụng thư viện System.Text.Json để đọc cấu trúc JSON
                    using var doc = System.Text.Json.JsonDocument.Parse(errorResult);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("error", out var errorElement))
                    {
                        ViewBag.ThongBaoLoi = errorElement.GetProperty("userMessage").GetString();
                    }

                    else
                    {
                        // Phòng hờ nếu API trả về cấu trúc khác
                        ViewBag.ThongBaoLoi = "Lỗi không xác định: " + errorResult;
                    }

                    return View(request);
                }
            }
            catch(Exception ex)
            {
                ViewBag.ThongBaoLoi = "Không thể kết nối đến API: " + ex.Message;
                return View(request);
            }
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public async Task<ActionResult> Login(LoginDTO request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                ViewBag.ThongBaoLoi = "Vui lòng nhập đầy đủ tài khoản và mật khẩu.";
                return View(request);
            }

            try
            {
                var jsonRequest = JsonSerializer.Serialize(request);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

          
                HttpResponseMessage response = await _httpClient.PostAsync("api/Account/Login", content);

                if (response.IsSuccessStatusCode) 
                {
                    string successResult = await response.Content.ReadAsStringAsync();
                    /*Chuyển đổi các HTTP content thành các chuỗi văn bản đã được chỉ định dưới dạng thao tác bất đồng bộ*/

                    using (JsonDocument doc = JsonDocument.Parse(successResult))
                    /*Phân tích văn bản đại diện cho 1 giá trị json duy nhất thành 1 JsonDocument*/
                    {
                        JsonElement root = doc.RootElement;
                        /*JsonElement đại diện cho một giá trị cụ thể trong cấu trúc JSON*/

                        JsonElement userInfo = root.GetProperty("userInfo");

                        string tenNguoiDung = userInfo.GetProperty("name").GetString();

                        string maNguoiDung = userInfo.GetProperty("id").GetString();

                        // Vì lỡ Khách hàng đăng nhập API không trả về Role, ta mặc định họ là "User".
                        string role = "User";
                        if (userInfo.TryGetProperty("role", out JsonElement roleElement))
                        {
                            role = roleElement.GetString();
                        }

                        HttpContext.Session.SetString("UserSession", successResult);
                        HttpContext.Session.SetString("CustomerId", maNguoiDung);
                        /*session, key(string), value(string)*/

                        // --- BẮT ĐẦU GẮN QUYỀN (CLAIMS) ---
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, tenNguoiDung),
                            new Claim("UserId", maNguoiDung),
                            new Claim(ClaimTypes.Role, role) // Gắn "Admin" hoặc "User"
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                        var authProperties = new AuthenticationProperties { IsPersistent = true };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);



                        TempData["ThongBao"] = $"Chào mừng {tenNguoiDung} đã đăng nhập thành công!";

                        if (role == "Admin")
                        {
                            return RedirectToAction("List", "Product");
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    string errorResult = await response.Content.ReadAsStringAsync();

                    try
                    {
                        // Ép kiểu chuỗi JSON lỗi vào Class ErrorResponse
                        var errorData = JsonSerializer.Deserialize<ErrorResponse>(errorResult, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true // Bỏ qua phân biệt hoa/thường
                        });

                        // Chỉ lấy đúng userMessage để hiển thị cho người dùng
                        if (errorData != null && errorData.error != null)
                        {
                            ViewBag.ThongBaoLoi = errorData.error.userMessage;
                        }
                        else
                        {
                            ViewBag.ThongBaoLoi = "Đăng nhập thất bại. Vui lòng kiểm tra lại.";
                        }
                    }
                    catch (JsonException)
                    {
                        // Đề phòng API sập trả về HTML thay vì JSON
                        ViewBag.ThongBaoLoi = "Lỗi từ máy chủ: " + errorResult;
                    }

                    return View(request);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ThongBaoLoi = "Không thể kết nối đến API: " + ex.Message;
                return View(request);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            // 2. ĐĂNG XUẤT KHỎI HỆ THỐNG PHÂN QUYỀN (CLAIMS) CỦA .NET
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["ThongBao"] = "Bạn đã đăng xuất thành công!";

            return RedirectToAction("Index", "Home");
        }
    }
}
