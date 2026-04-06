using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLBH.Common;

namespace QLBH.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IHttpClientFactory httpClientFactory, ILogger<CustomerController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        private HttpClient CreateApiClient() => _httpClientFactory.CreateClient("QLBH.API");

        private void SetApiUnavailableMessage()
        {
            TempData["Error"] = "Không kết nối được QLBH.API. Vui lòng chạy API trước, sau đó thử lại.";
        }

        public async Task<IActionResult> List()
        {
            try
            {
                var client = CreateApiClient();
                var response = await client.GetAsync("api/Customer/List");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<CustomerReq>>(jsonString) ?? new List<CustomerReq>();
                    return View(data);
                }

                TempData["Error"] = "Không lấy được danh sách khách hàng từ API.";
                return View(new List<CustomerReq>());
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Không thể gọi API danh sách khách hàng.");
                SetApiUnavailableMessage();
                return View(new List<CustomerReq>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CustomerReq());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerReq kh)
        {
            if (!ModelState.IsValid)
                return View(kh);

            try
            {
                var client = CreateApiClient();
                var response = await client.PostAsJsonAsync("api/Customer/Create", kh);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Thêm khách hàng thành công!";
                    return RedirectToAction("List");
                }

                var errorDetail = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Thêm thất bại. Chi tiết: " + errorDetail;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Không thể gọi API tạo khách hàng.");
                SetApiUnavailableMessage();
            }

            return View(kh);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var client = CreateApiClient();
                var response = await client.GetAsync($"api/Customer/GetById?id={id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<CustomerReq>(jsonString);
                    if (model != null) return View(model);
                }

                TempData["Error"] = "Không tìm thấy khách hàng cần sửa.";
                return RedirectToAction("List");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Không thể gọi API lấy chi tiết khách hàng {CustomerId}.", id);
                SetApiUnavailableMessage();
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerReq kh)
        {
            if (!ModelState.IsValid)
                return View(kh);

            try
            {
                var client = CreateApiClient();
                var response = await client.PostAsJsonAsync("api/Customer/Edit", kh);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật khách hàng thành công!";
                    return RedirectToAction("List");
                }

                var errorDetail = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Cập nhật thất bại. Chi tiết: " + errorDetail;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Không thể gọi API cập nhật khách hàng {CustomerId}.", kh.Id);
                SetApiUnavailableMessage();
            }

            return View(kh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var client = CreateApiClient();
                var response = await client.DeleteAsync($"api/Customer/Delete?id={id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Xóa khách hàng thành công!";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    TempData["Error"] = "Xóa thất bại! Chi tiết: " + error;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Không thể gọi API xóa khách hàng {CustomerId}.", id);
                SetApiUnavailableMessage();
            }

            return RedirectToAction("List");
        }
    }
}
