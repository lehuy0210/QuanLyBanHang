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
                /* JsonSerializer Cung cấp chức năng chuyển đổi các đối tượng hoặc kiểu giá trị thành định dạng JSON, 
                và chuyển đổi ngược JSON lại thành các đối tượng hoặc kiểu giá trị */
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("api/Customer/Create", content);
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
                    /*Chuyển đổi các HTTP content thành các chuỗi văn bản đã được chỉ định dưới dạng thao tác bất đồng bộ*/

                    using (JsonDocument doc = JsonDocument.Parse(successResult))
                    /*Phân tích văn bản đại diện cho 1 giá trị json duy nhất thành 1 JsonDocument*/
                    {
                        JsonElement root = doc.RootElement;
                        /*JsonElement đại diện cho một giá trị cụ thể trong cấu trúc JSON*/

                        string tenKhachHang = root.GetProperty("userInfo").GetProperty("name").GetString();

                        string maKhachHang = root.GetProperty("userInfo").GetProperty("id").GetString();

                        HttpContext.Session.SetString("UserSession", successResult);
                        HttpContext.Session.SetString("CustomerId", maKhachHang);
                        /*session, key(string), value(string)*/

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
