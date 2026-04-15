using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using QLBH.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddDistributedMemoryCache();

// 1. Thêm dịch vụ Authentication (Cookie)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Đường dẫn tới trang đăng nhập
        options.AccessDeniedPath = "/Home/AccessDenied"; // Đường dẫn khi user không có quyền Admin

        options.Cookie.Name = "Nhom20_Auth"; // Đặt tên riêng để không bị lẫn với đồ án khác
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Sau 30 phút không làm gì sẽ tự logout
        options.SlidingExpiration = true; // Nếu có hoạt động thì tự động gia hạn thêm 30 phút

        // QUAN TRỌNG: Cookie sẽ bị xóa khi tắt trình duyệt (không lưu vĩnh viễn)
        options.Cookie.MaxAge = null;
    });

builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session sẽ sống 30 phút nếu người dùng không tương tác
    options.Cookie.HttpOnly = true;                 // Bảo mật: Không cho Javascript đọc cookie này
    options.Cookie.IsEssential = true;              // Đánh dấu cookie này là bắt buộc (vượt qua các trình chặn cookie)
    options.Cookie.MaxAge = null;            // Đảm bảo Cookie không được lưu cứng xuống ổ cứng (Persistent Cookie)
    // Khi không set MaxAge hay Expiration, trình duyệt sẽ tự hiểu đây là "Session Cookie"
    // và sẽ tự xóa khi người dùng đóng toàn bộ trình duyệt.
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
// 2. Phải gọi UseAuthentication trước UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
