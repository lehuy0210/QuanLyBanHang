using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLBH.Common;
using QLBH.DAL;

namespace QLBH.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<OrderController> _logger;
        private readonly QLBH_DBContext _context;

        public OrderController(IHttpClientFactory httpClientFactory, ILogger<OrderController> logger, QLBH_DBContext context)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _context = context;
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
                var response = await client.GetAsync("api/Order/List");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<OrderReq>>(jsonString) ?? new List<OrderReq>();
                    return View(data);
                }

                TempData["Error"] = "Không lấy được danh sách đơn hàng từ API.";
                return View(new List<OrderReq>());
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Không thể gọi API danh sách đơn hàng.");
                SetApiUnavailableMessage();
                return View(new List<OrderReq>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var client = CreateApiClient();
                var response = await client.GetAsync($"api/Order/GetById?id={id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<OrderReq>(jsonString);
                    if (model != null) return View(model);
                }

                TempData["Error"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction("List");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Không thể gọi API chi tiết đơn hàng {OrderId}.", id);
                SetApiUnavailableMessage();
                return RedirectToAction("List");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new OrderReq
            {
                OrderDate = DateTime.Now,
                Customers = _context.Customers
                    .Select(c => new CustomerReq { Id = c.CustomerId.Trim(), Name = c.ContactName ?? c.CompanyName })
                    .ToList()
            };

            ViewBag.Products = _context.Products
                .Where(p => !p.Discontinued)
                .Select(p => new { p.ProductId, p.ProductName, p.UnitPrice })
                .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderReq order)
        {
            try
            {
                var client = CreateApiClient();
                var response = await client.PostAsJsonAsync("api/Order/Create", order);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Tạo đơn hàng thành công!";
                    return RedirectToAction("List");
                }

                var errorDetail = await response.Content.ReadAsStringAsync();
                TempData["Error"] = "Tạo đơn hàng thất bại. Chi tiết: " + errorDetail;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Không thể gọi API tạo đơn hàng.");
                SetApiUnavailableMessage();
            }

            order.Customers = _context.Customers
                .Select(c => new CustomerReq { Id = c.CustomerId.Trim(), Name = c.ContactName ?? c.CompanyName })
                .ToList();
            ViewBag.Products = _context.Products
                .Where(p => !p.Discontinued)
                .Select(p => new { p.ProductId, p.ProductName, p.UnitPrice })
                .ToList();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var client = CreateApiClient();
                var response = await client.DeleteAsync($"api/Order/Delete?id={id}");

                if (response.IsSuccessStatusCode)
                    TempData["Success"] = "Xóa đơn hàng thành công!";
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    TempData["Error"] = "Xóa thất bại! Chi tiết: " + error;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Không thể gọi API xóa đơn hàng {OrderId}.", id);
                SetApiUnavailableMessage();
            }

            return RedirectToAction("List");
        }
    }
}
