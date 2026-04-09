using Microsoft.EntityFrameworkCore;
using QLBH.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session sẽ sống 30 phút nếu người dùng không tương tác
    options.Cookie.HttpOnly = true;                 // Bảo mật: Không cho Javascript đọc cookie này
    options.Cookie.IsEssential = true;              // Đánh dấu cookie này là bắt buộc (vượt qua các trình chặn cookie)
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}"
    );

app.Run();
