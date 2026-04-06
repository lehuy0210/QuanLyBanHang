using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLBH.Common;
using QLBH.DAL;
using QLBH.DAL.Models;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using static System.Net.WebRequestMethods;

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

        private HttpClient CreateApiClient()
        {
            return _httpClientFactory.CreateClient("QLBH.API");
        }

        private void SetApiUnavailableMessage()
        {
            TempData["Error"] = "Không kết nối được QLBH.API. Vui lòng chạy API trước, sau đó thử lại.";
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
            try
            {
                var client = CreateApiClient();
                var response = await client.PostAsJsonAsync("api/Product/Create", sp);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Them san pham thanh cong.";
                    return RedirectToAction("Create");
                }

                var errorDetail = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Them that bai. Chi tiet: " + errorDetail;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Khong the goi API tao san pham.");
                SetApiUnavailableMessage();
            }

            sp.Categories = _context.Categories.Select(c => new CategoryReq { Id = c.CategoryId, Name = c.CategoryName }).ToList();
            sp.Suppliers = _context.Suppliers.Select(s => new SupplierReq { Id = s.SupplierId, Name = s.CompanyName }).ToList();

            return View(sp);
        }

        public async Task<IActionResult> List()
        {
            try
            {
                var client = CreateApiClient();
                var response = await client.PostAsync("api/Product/List", null);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var dt = JsonConvert.DeserializeObject<DataTable>(jsonString) ?? new DataTable();
                    return View(dt);
                }

                TempData["Error"] = "Khong lay duoc danh sach san pham tu API.";
                return View(new DataTable());
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Khong the goi API danh sach san pham.");
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
                    TempData["Error"] = "Khong tim thay san pham can sua.";
                    return RedirectToAction("List");
                }

                model.Categories = _context.Categories.Select(c => new CategoryReq { Id = c.CategoryId, Name = c.CategoryName }).ToList();
                model.Suppliers = _context.Suppliers.Select(s => new SupplierReq { Id = s.SupplierId, Name = s.CompanyName }).ToList();

                return View(model);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Khong the goi API lay chi tiet san pham {ProductId}.", id);
                SetApiUnavailableMessage();
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductReq sp)
        {
            try
            {
                var client = CreateApiClient();
                var response = await client.PostAsJsonAsync("api/Product/Edit", sp);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cap nhat san pham thanh cong.";
                    return RedirectToAction("List");
                }

                var errorDetail = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Cap nhat that bai. Chi tiet: " + errorDetail;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Khong the goi API cap nhat san pham {ProductId}.", sp.Id);
                SetApiUnavailableMessage();
            }

            sp.Categories = _context.Categories.Select(c => new CategoryReq { Id = c.CategoryId, Name = c.CategoryName }).ToList();
            sp.Suppliers = _context.Suppliers.Select(s => new SupplierReq { Id = s.SupplierId, Name = s.CompanyName }).ToList();

            return View(sp);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var client = CreateApiClient();
                var response = await client.PostAsync($"api/Product/Delete?id={id}", null);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Xoa san pham thanh cong!";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    TempData["Error"] = "Xoa that bai! Chi tiet: " + error;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Khong the goi API xoa san pham {ProductId}.", id);
                SetApiUnavailableMessage();
            }

            return RedirectToAction("List");
        }
    }
}
