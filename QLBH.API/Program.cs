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
// check cnstr
var connectionString = builder.Configuration.GetConnectionString("cnstr");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'cnstr' in QLBH.API/appsettings.json is not configured.");
}

builder.Services.AddDbContext<QLBH_DBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<ProductDAL>(_ => new ProductDAL(connectionString));
builder.Services.AddScoped<ProductBLL>();

var app = builder.Build();
var configuredUrls = builder.Configuration["ASPNETCORE_URLS"] ?? builder.Configuration["urls"];
var hasHttpsBinding = !string.IsNullOrWhiteSpace(configuredUrls)
                      && configuredUrls
                          .Split(';', StringSplitOptions.RemoveEmptyEntries)
                          .Any(url => url.TrimStart().StartsWith("https://", StringComparison.OrdinalIgnoreCase));

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (hasHttpsBinding)
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
