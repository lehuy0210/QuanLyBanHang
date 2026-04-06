# Quản Lý Bán Hàng (N-Layer Architecture)

Dự án Hệ Thống Quản Lý Bán Hàng sử dụng kiến trúc N-Layer (Web API + UI + BLL + DAL) với nền tảng .NET 8.

## Cấu Trúc Thư Mục

- **`QLBH.API`**: Backend API Server cung cấp các endpoints (Controllers) giao tiếp với Web UI. Đảm nhiệm việc kết nối frontend và business logic.
- **`QLBH.BLL`**: Business Logic Layer (Tầng nghiệp vụ). Chứa xử lý logic, tính toán, và chuyển tiếp dữ liệu đến DAL.
- **`QLBH.DAL`**: Data Access Layer (Tầng truy cập dữ liệu). Đảm nhiệm kết nối cơ sở dữ liệu `Northwind` (sử dụng SqlClient - ADO.NET).
- **`QLBH.Common`**: Chứa các lớp chung (DTOs, Req/Res models, ApiResponse) được chia sẻ giữa các tầng.
- **`QLBH.Web`**: Ứng dụng Frontend (Web/MVC) kết nối và tiêu thụ APIs.

## Yêu Cầu Hệ Thống
- .NET 8.0 SDK
- SQL Server (Sử dụng cơ sở dữ liệu `Northwind`)

## Cách Cài Đặt và Chạy
1. Cập nhật chuỗi kết nối (Connection String) `cnstr` bên trong file `QLBH.API/appsettings.json` để trỏ vào SQL Server `Northwind` của bạn.
2. Build solution: 
   ```bash
   dotnet build
   ```
3. Chạy API Server:
   ```bash
   cd QLBH.API
   dotnet run
   ```
4. Chạy Web UI:
   ```bash
   cd QLBH.Web
   dotnet run
   ```
