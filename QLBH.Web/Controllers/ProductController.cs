using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLBH.DTO;
using QLBH.DAL;
using QLBH.DAL.Models;
using System.Data;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace QLBH.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private async Task LoadDropdownData(ProductDTO model)
        {
            var client = _httpClientFactory.CreateClient();

            var resCat = await client.GetAsync("http://localhost:5003/api/Product/GetCategories");
            if (resCat.IsSuccessStatusCode)
            {
                var jsonCat = await resCat.Content.ReadAsStringAsync();
                model.Categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(jsonCat);
            }

            var resSup = await client.GetAsync("http://localhost:5003/api/Product/GetSuppliers");
            if (resSup.IsSuccessStatusCode)
            {
                var jsonSup = await resSup.Content.ReadAsStringAsync();
                model.Suppliers = JsonConvert.DeserializeObject<List<SupplierDTO>>(jsonSup);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new ProductDTO();

            await LoadDropdownData(model);

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO sp)
        {
            var client = _httpClientFactory.CreateClient();

            string apiThemSanPham = "http://localhost:5003/api/Product/Create";

            var response = await client.PostAsJsonAsync(apiThemSanPham, sp);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Thêm sản phẩm thành công.";
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
            await LoadDropdownData(sp);

            return View(sp);
        }

        public async Task<IActionResult> List()
        {
            var client = _httpClientFactory.CreateClient();

            string apiProduct = "http://localhost:5003/api/Product/List";

            var response = await client.GetAsync(apiProduct);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonString);

                return View(dt);
            }

            return View(new DataTable());

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient();

            string apiLaySanPhamTheoId = $"http://localhost:5003/api/Product/GetById?id={id}";
            var response = await client.GetAsync(apiLaySanPhamTheoId);

            var model = new ProductDTO();
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<ProductDTO>(jsonString);
            }
            else
            {
                TempData["Error"] = "Không tìm thấy sản phẩm cần sửa.";
                return RedirectToAction("List");
            }

            await LoadDropdownData(model);

            return View(model);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(ProductDTO sp)
        {
            var client = _httpClientFactory.CreateClient();

            string apiSuaSanPham = "http://localhost:5003/api/Product/Edit";

            var response = await client.PutAsJsonAsync(apiSuaSanPham, sp);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Cập nhật sản phẩm thành công.";
                return RedirectToAction("List");
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

            await LoadDropdownData(sp);

            return View(sp);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();

            string apiXoaSanPham = $"http://localhost:5003/api/Product/Delete?id={id}";

            var response = await client.DeleteAsync(apiXoaSanPham);

            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync();
                TempData["Success"] = "Xóa sản phẩm thành công!";
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

    }
}
