using Microsoft.AspNetCore.Mvc;
using QLBH.Common;
using System.Text.Json;
using System.Text;

namespace QLBH.Web.Controllers
{
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
        public async Task<ActionResult> Register(CustomerReq request, string Matkhaunhaplai)
        {
            if (request.Password != Matkhaunhaplai)
            {
                ViewBag.ThongBaoLoi = "Mật khẩu nhập lại không khớp!";
                return View(request);
            }
            request.Address = request.Address ?? "";
            request.City = request.City ?? "";
            request.Country = request.Country ?? "";
            try
            {
                var jsonRequest = JsonSerializer.Serialize(request);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("api/Customer/Create", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["ThongBao"] = "Đăng ký thành công!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    string errorResult = await response.Content.ReadAsStringAsync();
                    ViewBag.ThongBaoLoi = $"API báo lỗi {(int)response.StatusCode}. Chi tiết: {errorResult}";
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

        public async Task<ActionResult> Login(LoginReq request)
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

                    using (JsonDocument doc = JsonDocument.Parse(successResult))
                    {
                        JsonElement root = doc.RootElement;

                        string tenKhachHang = root.GetProperty("userInfo").GetProperty("name").GetString();

                        HttpContext.Session.SetString("UserSession", successResult);

                        TempData["ThongBao"] = $"Chào mừng {tenKhachHang} đã đăng nhập thành công!";
                        return RedirectToAction("Index", "Home");
                    }
                }
                else 
                {
                   
                    string errorResult = await response.Content.ReadAsStringAsync();

                    ViewBag.ThongBaoLoi = errorResult;
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
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            TempData["ThongBao"] = "Bạn đã đăng xuất thành công!";

            return RedirectToAction("Index", "Home");
        }


    }
}
