using Microsoft.EntityFrameworkCore;
using QLBH.DAL.Models;
using QLBH.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH.DAL
{
    public class QLBH_DBContext: DbContext
    {
        public QLBH_DBContext(DbContextOptions<QLBH_DBContext> options) : base(options)
        {

        }

        public QLBH_DBContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Nhớ đổi chuỗi này thành chuỗi kết nối SQL Server đang xài
                optionsBuilder.UseSqlServer("Server=LAPTOP-5RU50CLF\\HUY;database=Northwind;user id=sa;password=123456;TrustServerCertificate=True;");
            }
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }

        public DbSet<CategoryDTO> CategoryReqs { get; set; }
        public DbSet<SupplierDTO> SupplierReqs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Trỏ đúng vào tên bảng gốc của Northwind
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderDetail>().ToTable("Order Details"); 
        }
    }
}
