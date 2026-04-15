using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLBH.Common;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;

namespace QLBH.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EmployeeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> List()
        {
            var client = _httpClientFactory.CreateClient();
            string apiEmployee = "http://localhost:5003/api/Employee/List";

            var response = await client.GetAsync(apiEmployee);

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
            var model = new EmployeeReq();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeReq nv)
        {
            var client = _httpClientFactory.CreateClient();
            string apiThemNV = "http://localhost:5003/api/Employee/Create";

            var response = await client.PostAsJsonAsync(apiThemNV, nv);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Thêm nhân viên thành công.";
                return RedirectToAction("Create");
            }
            else
            {
                var errorDetail = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Thêm thất bại. Chi tiết: " + errorDetail;
            }
            return View(nv);
        }

        [HttpGet("Employee/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();
            string apiXoaNhanVien = $"http://localhost:5003/api/Employee/Delete?id={id}";

            var response = await client.DeleteAsync(apiXoaNhanVien);

            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync();
                TempData["Success"] = "Xóa nhân viên thành công!";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Xóa thất bại! Chi tiết: " + error;
            }
            return RedirectToAction("List");
        }

        [HttpGet("Employee/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient();
            string apiLayNVTheoId = $"http://localhost:5003/api/Employee/GetById?id={id}";

            var response = await client.GetAsync(apiLayNVTheoId);
            var model = new EmployeeReq();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<EmployeeReq>(jsonString);
            }
            else
            {
                TempData["Error"] = "Không tìm thấy nhân viên cần sửa.";
                return RedirectToAction("List");
            }

            return View(model);
        }

        [HttpPost("Employee/Edit/{id?}")]
        public async Task<IActionResult> Edit(EmployeeReq nv)
        {
            var client = _httpClientFactory.CreateClient();
            string apiSuaNhanVien = "http://localhost:5003/api/Employee/Edit";

            var response = await client.PutAsJsonAsync(apiSuaNhanVien, nv);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Cập nhật nhân viên thành công.";
                return RedirectToAction("List");
            }
            else
            {
                var errorDetail = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Cập nhật thất bại. Chi tiết: " + errorDetail;
            }

            return View(nv);
        }
    }
}