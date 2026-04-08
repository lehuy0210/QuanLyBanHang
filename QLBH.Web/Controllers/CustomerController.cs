using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLBH.Common;
using System.Data;
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

        public async Task<IActionResult> List()
        {
            var client = _httpClientFactory.CreateClient();

            string apiCustomer = "http://localhost:5003/api/Customer/List";

            var response = await client.GetAsync(apiCustomer);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonString);

                return View(dt);
            }

            return View(new DataTable());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CustomerReq();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerReq kh)
        {
            var client = _httpClientFactory.CreateClient();

            string apiThemKH = "http://localhost:5003/api/Customer/Create";

            var response = await client.PostAsJsonAsync(apiThemKH, kh);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Thêm khách hàng thành công.";
                return RedirectToAction("Create");
            }
            else
            {
                var errorDetail = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Thêm thất bại. Chi tiết: " + errorDetail;
            }
            return View(kh);
        }

        [HttpGet("Customer/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var client = _httpClientFactory.CreateClient();

            string apiXoaKhachHang = $"http://localhost:5003/api/Customer/Delete?id={id}";

            var response = await client.DeleteAsync(apiXoaKhachHang);

            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync();
                TempData["Success"] = "Xóa khách hàng thành công!";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Xóa thất bại! Chi tiết: " + error;
            }
            return RedirectToAction("List");
        }

        [HttpGet("Customer/Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var client = _httpClientFactory.CreateClient();

            string apiLayKHTheoId = $"http://localhost:5003/api/Customer/GetById?id={id}";
            var response = await client.GetAsync(apiLayKHTheoId);

            var model = new CustomerReq();
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<CustomerReq>(jsonString);
            }
            else
            {
                TempData["Error"] = "Không tìm thấy khách hàng cần sửa.";
                return RedirectToAction("List");
            }

            return View(model);
        }


        [HttpPost("Edit/{id?}")]
        public async Task<IActionResult> Edit(CustomerReq kh)
        {
            var client = _httpClientFactory.CreateClient();

            string apiSuaKhachHang = "http://localhost:5003/api/Customer/Edit";

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
    }
}
