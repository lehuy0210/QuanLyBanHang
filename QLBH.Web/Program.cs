using Microsoft.EntityFrameworkCore;
using QLBH.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QLBH_DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cnstr")));
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=List}/{id?}"
    );

app.Run();
