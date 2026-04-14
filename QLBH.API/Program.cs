using Microsoft.EntityFrameworkCore;
using QLBH.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<QLBH_DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cnstr")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()   // Cho phép mọi tên miền/cổng (kể cả MVC của bạn)
               .AllowAnyMethod()   // Cho phép mọi lệnh (GET, POST, PUT, DELETE)
               .AllowAnyHeader();  // Cho phép mọi loại dữ liệu (JSON...)
    });
});


var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
