using Microsoft.EntityFrameworkCore;
using QLBH.DAL.Models;
using QLBH.DTO;

namespace QLBH.DAL
{
    public class QLBH_DBContext : DbContext
    {
        public QLBH_DBContext(DbContextOptions<QLBH_DBContext> options) : base(options) { }
        public QLBH_DBContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=LAPTOP-5RU50CLF\\HUY;database=Northwind;user id=sa;password=123456;TrustServerCertificate=True;");
            }
        }

        // --- Các Bảng (Tables) ---
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }

        // --- CÁC VIEW (Bắt buộc phải có DbSet để truy vấn) ---
        public DbSet<ProductDTO> ProductDTOs { get; set; }
        public DbSet<CustomerDTO> CustomerDTOs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map tên bảng cho đúng
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderDetail>().ToTable("Order Details");

            // Fix lỗi khóa chính phức hợp
            modelBuilder.Entity<OrderDetail>().HasKey(od => new { od.OrderId, od.ProductId });

            // CẤU HÌNH KIỂU DỮ LIỆU MONEY (Fix cảnh báo truncation)
            modelBuilder.Entity<Order>().Property(o => o.Freight).HasColumnType("money");
            modelBuilder.Entity<OrderDetail>().Property(od => od.UnitPrice).HasColumnType("money");
            modelBuilder.Entity<Product>().Property(p => p.UnitPrice).HasColumnType("money");
            modelBuilder.Entity<ProductDTO>().Property(p => p.Price).HasColumnType("money");

            // Mapping Views
            modelBuilder.Entity<ProductDTO>(entity => {
                entity.HasNoKey();
                entity.ToView("DanhSachSanPham");
                entity.Property(p => p.Id).HasColumnName("ProductID");
                entity.Property(p => p.Name).HasColumnName("ProductName");
                entity.Property(p => p.Price).HasColumnName("UnitPrice");
                entity.Property(p => p.Quantity).HasColumnName("QuantityPerUnit");
            });

            modelBuilder.Entity<CustomerDTO>(entity => {
                entity.HasNoKey();
                entity.ToView("DanhSachKhachHang");
                entity.Property(c => c.Id).HasColumnName("CustomerID");
                entity.Property(c => c.Name).HasColumnName("ContactName");
            });
        }
    }
}