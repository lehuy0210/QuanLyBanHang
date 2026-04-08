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
    }
}
