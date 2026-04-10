using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;
using QLBH.Common;               
using System.Collections.Generic;
using System.Linq;            
using System.Net.Http;
using System.Net.Http.Json;    
using System.Text.Json;     
using System.Threading.Tasks;
namespace QLBH.Web.Controllers
{
    public class OrderController : Controller
    {
        private const string CART_KEY = "CartSession";

        [HttpPost]

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity, string productName, decimal unitPrice)
        {
            // 1. Lấy URL trang trước đó để quay lại sau khi xử lý
            string urlTruocDo = Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(urlTruocDo)) urlTruocDo = "/";

            // 2. KIỂM TRA ĐĂNG NHẬP
            string userSessionData = HttpContext.Session.GetString("UserSession");

            if (string.IsNullOrEmpty(userSessionData))
            {
                TempData["ThongBao"] = "Vui lòng đăng nhập để sử dụng giỏ hàng!";
                return Redirect(urlTruocDo);
            }

            // 3. XỬ LÝ GIỎ HÀNG
            var cart = GetCartFromSession();
            var item = cart.FirstOrDefault(p => p.ProductId == productId);

            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                string nameToSave = productName;

                // Chỉ gọi API nếu productName truyền vào bị trống
                if (string.IsNullOrEmpty(nameToSave))
                {
                    try
                    {
                        using var client = new HttpClient();
                        var response = await client.GetAsync($"http://localhost:5003/api/Product/GetByID?id={productId}");
                        if (response.IsSuccessStatusCode)
                        {
                            var productData = await response.Content.ReadFromJsonAsync<ProductReq>();
                            nameToSave = productData?.Name ?? "Sản phẩm không tên";
                        }
                    }
                    catch
                    {
                        nameToSave = "Lỗi kết nối API";
                    }
                }

                cart.Add(new OrderReq
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    ProductName = nameToSave
                });
            }

            // 4. LƯU SESSION VÀ TRẢ VỀ
            SaveCartToSession(cart);
            TempData["ThongBao"] = "Đã thêm vào giỏ hàng!";

            return Redirect(urlTruocDo);
            //return Content($"WEB ĐÃ NHẬN: ID={productId}, Tên={productName}, Giá={unitPrice}");
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCartFromSession();
            if (!cart.Any()) return BadRequest("Giỏ hàng trống");

            // Lấy CustomerId từ Session (giả sử đã lưu khi đăng nhập)
            string customerId = HttpContext.Session.GetString("CustomerId") ?? "GUEST";

            // Dùng HttpClient để gửi dữ liệu sang API
            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync($"https://localhost:7001/api/Order/Create?customerId={customerId}", cart);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonString);

                int newId = 0;
                if (doc.RootElement.TryGetProperty("OrderId", out var idProp))
                {
                    newId = idProp.GetInt32();
                }

                HttpContext.Session.Remove(CART_KEY); // Xóa giỏ hàng sau khi mua xong
                TempData["ThongBao"] = "Đặt hàng thành công!";

                return RedirectToAction("Success", new { id = newId });
            }

            return View("Error");
        }

        private List<OrderReq> GetCartFromSession()
        {
            try
            {
                var data = HttpContext.Session.GetString(CART_KEY);
                if (string.IsNullOrEmpty(data)) return new List<OrderReq>();
                return JsonSerializer.Deserialize<List<OrderReq>>(data) ?? new List<OrderReq>();
            }
            catch
            {
                // Nếu dữ liệu cũ trong Session bị lỗi format, xóa nó đi để tạo mới
                HttpContext.Session.Remove(CART_KEY);
                return new List<OrderReq>();
            }
        }

        private void SaveCartToSession(List<OrderReq> cart) =>
            HttpContext.Session.SetString(CART_KEY, JsonSerializer.Serialize(cart));

        public IActionResult Success(int id)
        {
            ViewBag.OrderId = id;
            return View(); 
        }

        public IActionResult Index()
        {
            // Lấy giỏ hàng từ Session để truyền vào View
            var cart = GetCartFromSession();

            // Nếu giỏ hàng trống, báo lỗi hoặc cho về Home
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            return View(cart); // Lệnh này sẽ mở file Views/Order/Index.cshtml
        }

    }
}
