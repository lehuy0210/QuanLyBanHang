using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLBH.Common;
using QLBH.DAL;
using QLBH.DAL.Models;
using System.Data;
using System.Net.Http;
using static System.Net.WebRequestMethods;

namespace QLBH.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly QLBH_DBContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductController(QLBH_DBContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = new ProductReq();

            model.Categories = _context.Categories.Select(c => new CategoryReq { Id = c.CategoryId, Name = c.CategoryName }).ToList();
            model.Suppliers = _context.Suppliers.Select(s => new SupplierReq { Id = s.SupplierId, Name = s.CompanyName }).ToList();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductReq sp)
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
                var errorDetail = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Thêm thất bại. Chi tiết: " + errorDetail;
            }
            sp.Categories = _context.Categories.Select(c => new CategoryReq { Id = c.CategoryId, Name = c.CategoryName }).ToList();
            sp.Suppliers = _context.Suppliers.Select(s => new SupplierReq { Id = s.SupplierId, Name = s.CompanyName }).ToList();

            return View(sp);
        }

        public async Task<IActionResult> List()
        {
            var client = _httpClientFactory.CreateClient();

            string apiProduct = "http://localhost:5003/api/Product/List";

            var response = await client.PostAsync(apiProduct, null);

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

            var model = new ProductReq();
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<ProductReq>(jsonString);
            }
            else
            {
                TempData["Error"] = "Không tìm thấy sản phẩm cần sửa.";
                return RedirectToAction("List");
            }

            model.Categories = _context.Categories.Select(c => new CategoryReq { Id = c.CategoryId, Name = c.CategoryName }).ToList();
            model.Suppliers = _context.Suppliers.Select(s => new SupplierReq { Id = s.SupplierId, Name = s.CompanyName }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductReq sp)
        {
            var client = _httpClientFactory.CreateClient();

            string apiSuaSanPham = "http://localhost:5003/api/Product/Edit";

            var response = await client.PostAsJsonAsync(apiSuaSanPham, sp);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Cập nhật sản phẩm thành công.";
                return RedirectToAction("List");
            }
            else
            {
                var errorDetail = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Cập nhật thất bại. Chi tiết: " + errorDetail;
            }

            sp.Categories = _context.Categories.Select(c => new CategoryReq { Id = c.CategoryId, Name = c.CategoryName }).ToList();
            sp.Suppliers = _context.Suppliers.Select(s => new SupplierReq { Id = s.SupplierId, Name = s.CompanyName }).ToList();

            return View(sp);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();

            string apiXoaSanPham = $"http://localhost:5003/api/Product/Delete?id={id}";

            var response = await client.PostAsync(apiXoaSanPham, null);

            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStringAsync();
                TempData["Success"] = "Xóa sản phẩm thành công!";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Xóa thất bại! Chi tiết: " + error;
            }
            return RedirectToAction("List");
        }

    }
}
