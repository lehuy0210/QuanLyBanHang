using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLBH.DTO;
using System.Data;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace QLBH.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List()
        {
            var client = _httpClientFactory.CreateClient();

            string apiCustomer = "http://localhost:5003/api/Customer";

            var response = await client.GetAsync(apiCustomer);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonString);

                return View(dt);
            }

            return View(new DataTable());
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CustomerDTO();

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CustomerDTO kh)
        {
            var client = _httpClientFactory.CreateClient();

            string apiThemKH = "http://localhost:5003/api/Customer";

            var response = await client.PostAsJsonAsync(apiThemKH, kh);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Thêm khách hàng thành công.";
                return RedirectToAction("Create");
            }
            else
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                try
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(errorJson);
                    TempData["Error"] = doc.RootElement.GetProperty("error").GetProperty("userMessage").GetString();
                }
                catch { TempData["Error"] = "Thêm thất bại. Chi tiết: " + errorJson; }
            }
            return View(kh);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Customer/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var client = _httpClientFactory.CreateClient();

            string apiXoaKhachHang = $"http://localhost:5003/api/Customer/{id}";

            var response = await client.DeleteAsync(apiXoaKhachHang);

            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync();
                TempData["Success"] = "Xóa khách hàng thành công!";
            }
            else
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                try
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(errorJson);
                    TempData["Error"] = doc.RootElement.GetProperty("error").GetProperty("userMessage").GetString();
                }
                catch { TempData["Error"] = "Xóa thất bại! Chi tiết: " + errorJson; }
            }
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Customer/Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var client = _httpClientFactory.CreateClient();

            string apiLayKHTheoId = $"http://localhost:5003/api/Customer/{id}";
            var response = await client.GetAsync(apiLayKHTheoId);

            var model = new CustomerDTO();
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<CustomerDTO>(jsonString);
            }
            else
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                try
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(errorJson);
                    TempData["Error"] = doc.RootElement.GetProperty("error").GetProperty("userMessage").GetString();
                }
                catch { TempData["Error"] = "Cập nhật thất bại. Chi tiết: " + errorJson; }
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Customer/Edit/{id?}")]
        public async Task<IActionResult> Edit(CustomerDTO kh,string id)
        {
            var client = _httpClientFactory.CreateClient();

            string apiSuaKhachHang = $"http://localhost:5003/api/Customer/{id}";

            var response = await client.PutAsJsonAsync(apiSuaKhachHang, kh);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Cập nhật khách hàng thành công.";
                return RedirectToAction("List");
            }
            else
            {
                var errorDetail = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Cập nhật thất bại. Chi tiết: " + errorDetail;
            }

            return View(kh);
        }

        [Authorize(Roles = "User")]
        [HttpGet("Customer/OrderDetail")]
        public async Task<IActionResult> OrderDetail()
        {
            string currentUserId = User.FindFirst("UserId")?.Value;

            List<OrderDTO> model = new List<OrderDTO>();

            var client = _httpClientFactory.CreateClient();

            string apiDonHangTheoKH = $"http://localhost:5003/api/Order/Customer/{currentUserId}";

            var response = await client.GetAsync(apiDonHangTheoKH);

            if(response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();

                var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                model = System.Text.Json.JsonSerializer.Deserialize<List<OrderDTO>>(data, options);
            }
            else
            {
                var errorDetail = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Load đơn hàng thất bại. Chi tiết: " + errorDetail;
            }
            return View(model);

        }
    }
}
