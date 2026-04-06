using System;
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
    throw new InvalidOperationException("Connection string 'cnstr' in QLBH.API/appsettings is not configured.");
}

builder.Services.AddDbContext<QLBH_DBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<ProductDAL>(_ => new ProductDAL(connectionString));
builder.Services.AddScoped<ProductBLL>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
