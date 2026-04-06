using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QLBH.BLL;
using QLBH.DAL;

var builder = WebApplication.CreateBuilder(args);

// Khởi tạo dịch vụ API
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS - cho phép Web project gọi API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWeb", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Connection string
var connectionString = builder.Configuration.GetConnectionString("cnstr");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'cnstr' in QLBH.API/appsettings.json is not configured.");
}

builder.Services.AddDbContext<QLBH_DBContext>(options =>
    options.UseSqlServer(connectionString));

// DI Registration
builder.Services.AddScoped<CustomerDAL>(_ => new CustomerDAL(connectionString));
builder.Services.AddScoped<CustomerBLL>();
builder.Services.AddScoped<ProductDAL>(_ => new ProductDAL(connectionString));
builder.Services.AddScoped<ProductBLL>();
builder.Services.AddScoped<OrderDAL>(_ => new OrderDAL(connectionString));
builder.Services.AddScoped<OrderBLL>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowWeb");
app.UseAuthorization();
app.MapControllers();

app.Run();
