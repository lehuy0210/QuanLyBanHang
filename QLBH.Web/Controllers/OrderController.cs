using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;
using QLBH.DTO;               
using System.Collections.Generic;
using System.Data;
using System.Linq;            
using System.Net.Http.Json;    
using System.Text.Json;     
using System.Threading.Tasks;
namespace QLBH.Web.Controllers
{
    public class OrderController : Controller
    {
        private const string CART_KEY = "CartSession";

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity, string productName, decimal unitPrice)
        {
            // 1. Lấy URL trang trước đó để quay lại sau khi xử lý
            string urlTruocDo = Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(urlTruocDo)) urlTruocDo = "/";

            // Kiểm tra xem đây có phải là yêu cầu AJAX không
            bool isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            var userJson = HttpContext.Session.GetString("UserSession");

            // 2. KIỂM TRA ĐĂNG NHẬP
            if (!User.Identity.IsAuthenticated || string.IsNullOrEmpty(userJson))
            {
                TempData["ThongBao"] = "Vui lòng đăng nhập trước khi mua hàng!";
                if (isAjax) return Json(new { success = false, message = "Vui lòng đăng nhập trước khi mua hàng!" }); // Trả về JSON cho AJAX
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
                                var productData = await response.Content.ReadFromJsonAsync<ProductDTO>();
                                nameToSave = productData.Name;
                            }
                        }
                        catch
                        {
                            nameToSave = "Lỗi kết nối API";
                        }
                    }

                    cart.Add(new OrderDTO
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
            if (isAjax) return Json(new { success = true, message = "Đã thêm vào giỏ hàng!" }); // Trả về JSON cho AJAX

            return Redirect(urlTruocDo);
                //return Content($"WEB ĐÃ NHẬN: ID={productId}, Tên={productName}, Giá={unitPrice}");
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCartFromSession();
            if (!cart.Any()) return BadRequest("Giỏ hàng trống");

            string customerId = HttpContext.Session.GetString("CustomerId");


            // 2. Chặn dự phòng: Nhỡ Session hết hạn (do treo máy lâu) thì bắt đăng nhập lại
            if (string.IsNullOrEmpty(customerId))
            {
                TempData["ThongBao"] = "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại để thanh toán!";
                // Sửa đường dẫn này về trang Login thực tế của bạn nếu cần
                return RedirectToAction("Index", "Home");
            }

            // Dùng HttpClient để gửi dữ liệu sang API
            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync($"http://localhost:5003/api/Order?customerId={customerId}", cart);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonString);

                int newId = 0;
                if (doc.RootElement.TryGetProperty("orderId", out var idProp))
                {
                    newId = idProp.GetInt32();
                }

                HttpContext.Session.Remove(CART_KEY); // Xóa giỏ hàng sau khi mua xong
                TempData["ThongBao"] = "Đặt hàng thành công!";

                return RedirectToAction("Success", new { id = newId });
            }
            else
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                try
                {
                    using var doc = JsonDocument.Parse(errorJson);
                    TempData["ThongBao"] = doc.RootElement.GetProperty("error").GetProperty("userMessage").GetString();
                }
                catch { TempData["ThongBao"] = "Lỗi đặt hàng: " + errorJson; }

                return RedirectToAction("Index");
            }
        }

        private List<OrderDTO> GetCartFromSession()
        {
            try
            {
                var data = HttpContext.Session.GetString(CART_KEY);
                if (string.IsNullOrEmpty(data)) return new List<OrderDTO>();
                return JsonSerializer.Deserialize<List<OrderDTO>>(data) ?? new List<OrderDTO>();
            }
            catch
            {
                // Nếu dữ liệu cũ trong Session bị lỗi format, xóa nó đi để tạo mới
                HttpContext.Session.Remove(CART_KEY);
                return new List<OrderDTO>();
            }
        }

        private void SaveCartToSession(List<OrderDTO> cart) =>
            HttpContext.Session.SetString(CART_KEY, JsonSerializer.Serialize(cart));

        [Authorize(Roles = "User")]
        public IActionResult Success(int id)
        {
            ViewBag.OrderId = id;
            return View(); 
        }
        [Authorize(Roles = "User")]
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            // 1. Tạo sẵn một DataTable với các cột tương ứng
            DataTable dtOrder = new DataTable();
            dtOrder.Columns.Add("OrderID");
            dtOrder.Columns.Add("ContactName");
            dtOrder.Columns.Add("Quantity");
            dtOrder.Columns.Add("TotalPrice");
            dtOrder.Columns.Add("OrderDate");

            try
            {
                // 2. Dùng HttpClient để gọi API
                using var client = new HttpClient();

                // LƯU Ý: Sửa lại port 5003 thành port thực tế API của bạn đang chạy
                var response = await client.GetAsync("http://localhost:5003/api/Order");

                if (response.IsSuccessStatusCode)
                {
                    // Đọc kết quả JSON từ API trả về
                    var jsonString = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(jsonString);

                    // 3. Lặp qua từng object trong mảng JSON và nhét vào DataTable
                    foreach (var item in doc.RootElement.EnumerateArray())
                    {
                        DataRow row = dtOrder.NewRow();

                        // Chú ý: .NET tự động đổi chữ cái đầu thành viết thường (camelCase) khi trả về JSON
                        // Nên ở đây phải dùng "orderID", "contactName"... (chữ cái đầu viết thường)
                        row["OrderID"] = item.GetProperty("orderID").ToString();
                        row["ContactName"] = item.GetProperty("contactName").ToString();
                        row["Quantity"] = item.GetProperty("quantity").ToString();
                        row["TotalPrice"] = item.GetProperty("totalPrice").ToString();
                        row["OrderDate"] = item.GetProperty("orderDate").ToString();

                        dtOrder.Rows.Add(row);
                    }
                }
                else
                {
                    TempData["ThongBao"] = "Lỗi khi gọi API: " + response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                TempData["ThongBao"] = "Không thể kết nối đến API: " + ex.Message;
            }

            // 4. Ném DataTable đã chứa dữ liệu sang View
            return View(dtOrder);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Detail(int id)
        {
            List<OrderDTO> orderDetails = new List<OrderDTO>();

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"http://localhost:5003/api/Order/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    orderDetails = JsonSerializer.Deserialize<List<OrderDTO>>(data, options);
                }

                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Nếu API trả về 404
                    ViewBag.ErrorMessage = "Không tìm thấy đơn hàng này!";
                }
                else
                {
                    // Lỗi khác (500, 400...)
                    ViewBag.ErrorMessage = "Đã xảy ra lỗi khi kết nối tới hệ thống.";
                }
            }
            return View(orderDetails);
        }

    }
}
