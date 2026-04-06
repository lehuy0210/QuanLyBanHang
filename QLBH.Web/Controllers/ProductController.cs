using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLBH.Common;
using QLBH.DAL;
using System.Data;
using System.Net.Http.Json;

namespace QLBH.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly QLBH_DBContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            QLBH_DBContext context,
            IHttpClientFactory httpClientFactory,
            ILogger<ProductController> logger)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        private HttpClient CreateApiClient() => _httpClientFactory.CreateClient("QLBH.API");

        private void SetApiUnavailableMessage()
        {
            TempData["Error"] = "Không kết nối được QLBH.API. Vui lòng chạy API trước, sau đó thử lại.";
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ProductReq
            {
                Categories = _context.Categories
                    .Select(c => new CategoryReq { Id = c.CategoryId, Name = c.CategoryName }).ToList(),
                Suppliers = _context.Suppliers
                    .Select(s => new SupplierReq { Id = s.SupplierId, Name = s.CompanyName }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductReq sp)
        {
            if (!ModelState.IsValid)
            {
                sp.Categories = _context.Categories
                    .Select(c => new CategoryReq { Id = c.CategoryId, Name = c.CategoryName }).ToList();
                sp.Suppliers = _context.Suppliers
                    .Select(s => new SupplierReq { Id = s.SupplierId, Name = s.CompanyName }).ToList();
                return View(sp);
            }

            try
            {
                var client = CreateApiClient();
                var response = await client.PostAsJsonAsync("api/Product/Create", sp);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Thêm sản phẩm thành công!";
                    return RedirectToAction("Create");
                }

                var errorDetail = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Thêm thất bại. Chi tiết: " + errorDetail;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Không thể gọi API tạo sản phẩm.");
                SetApiUnavailableMessage();
            }

            sp.Categories = _context.Categories
                .Select(c => new CategoryReq { Id = c.CategoryId, Name = c.CategoryName }).ToList();
            sp.Suppliers = _context.Suppliers
                .Select(s => new SupplierReq { Id = s.SupplierId, Name = s.CompanyName }).ToList();
            return View(sp);
        }

        public async Task<IActionResult> List()
        {
            try
            {
                var client = CreateApiClient();
                var response = await client.GetAsync("api/Product/List");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var dt = JsonConvert.DeserializeObject<DataTable>(jsonString) ?? new DataTable();
                    return View(dt);
                }

                TempData["Error"] = "Không lấy được danh sách sản phẩm từ API.";
                return View(new DataTable());
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Không thể gọi API danh sách sản phẩm.");
                SetApiUnavailableMessage();
                return View(new DataTable());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var client = CreateApiClient();
                var response = await client.GetAsync($"api/Product/GetById?id={id}");

                ProductReq? model = null;
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<ProductReq>(jsonString);
                }

                if (model == null)
                {
                    TempData["Error"] = "Không tìm thấy sản phẩm cần sửa.";
                    return RedirectToAction("List");
                }

                model.Categories = _context.Categories
                    .Select(c => new CategoryReq { Id = c.CategoryId, Name = c.CategoryName }).ToList();
                model.Suppliers = _context.Suppliers
                    .Select(s => new SupplierReq { Id = s.SupplierId, Name = s.CompanyName }).ToList();

                return View(model);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Không thể gọi API lấy chi tiết sản phẩm {ProductId}.", id);
                SetApiUnavailableMessage();
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductReq sp)
        {
            if (!ModelState.IsValid)
            {
                sp.Categories = _context.Categories
                    .Select(c => new CategoryReq { Id = c.CategoryId, Name = c.CategoryName }).ToList();
                sp.Suppliers = _context.Suppliers
                    .Select(s => new SupplierReq { Id = s.SupplierId, Name = s.CompanyName }).ToList();
                return View(sp);
            }

            try
            {
                var client = CreateApiClient();
                var response = await client.PutAsJsonAsync("api/Product/Edit", sp);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction("List");
                }

                var errorDetail = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Cập nhật thất bại. Chi tiết: " + errorDetail;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Không thể gọi API cập nhật sản phẩm {ProductId}.", sp.Id);
                SetApiUnavailableMessage();
            }

            sp.Categories = _context.Categories
                .Select(c => new CategoryReq { Id = c.CategoryId, Name = c.CategoryName }).ToList();
            sp.Suppliers = _context.Suppliers
                .Select(s => new SupplierReq { Id = s.SupplierId, Name = s.CompanyName }).ToList();
            return View(sp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var client = CreateApiClient();
                var response = await client.DeleteAsync($"api/Product/Delete?id={id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Xóa sản phẩm thành công!";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    TempData["Error"] = "Xóa thất bại! Chi tiết: " + error;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Không thể gọi API xóa sản phẩm {ProductId}.", id);
                SetApiUnavailableMessage();
            }

            return RedirectToAction("List");
        }
    }
}
