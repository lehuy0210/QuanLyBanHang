using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;
using System.Text.RegularExpressions;

#nullable disable

namespace QLBH.DAL.Migrations
{
    public partial class Northwind : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                // --- 1. TẠO CÁC BẢNG (14 TABLES) ---
                migrationBuilder.Sql(@"
                CREATE TABLE [dbo].[Customers]([CustomerID] [nchar](5) NOT NULL PRIMARY KEY, [CompanyName] [nvarchar](40) NOT NULL, [ContactName] [nvarchar](30) NULL, [ContactTitle] [nvarchar](30) NULL, [Address] [nvarchar](60) NULL, [City] [nvarchar](15) NULL, [Region] [nvarchar](15) NULL, [PostalCode] [nvarchar](10) NULL, [Country] [nvarchar](15) NULL, [Phone] [nvarchar](24) NULL, [Fax] [nvarchar](24) NULL, [Username] [nvarchar](50) NULL, [Password] [nvarchar](255) NULL);
                CREATE TABLE [dbo].[Employees]([EmployeeID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, [LastName] [nvarchar](20) NOT NULL, [FirstName] [nvarchar](10) NOT NULL, [Title] [nvarchar](30) NULL, [TitleOfCourtesy] [nvarchar](25) NULL, [BirthDate] [datetime] NULL, [HireDate] [datetime] NULL, [Address] [nvarchar](60) NULL, [City] [nvarchar](15) NULL, [Region] [nvarchar](15) NULL, [PostalCode] [nvarchar](10) NULL, [Country] [nvarchar](15) NULL, [HomePhone] [nvarchar](24) NULL, [Extension] [nvarchar](4) NULL, [Photo] [image] NULL, [Notes] [ntext] NULL, [ReportsTo] [int] NULL, [PhotoPath] [nvarchar](255) NULL, [Username] [nvarchar](50) NULL, [Password] [nvarchar](255) NULL);
                CREATE TABLE [dbo].[Categories]([CategoryID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, [CategoryName] [nvarchar](15) NOT NULL, [Description] [ntext] NULL, [Picture] [image] NULL);
                CREATE TABLE [dbo].[Suppliers]([SupplierID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, [CompanyName] [nvarchar](40) NOT NULL, [ContactName] [nvarchar](30) NULL, [ContactTitle] [nvarchar](30) NULL, [Address] [nvarchar](60) NULL, [City] [nvarchar](15) NULL, [Region] [nvarchar](15) NULL, [PostalCode] [nvarchar](10) NULL, [Country] [nvarchar](15) NULL, [Phone] [nvarchar](24) NULL, [Fax] [nvarchar](24) NULL, [HomePage] [ntext] NULL);
                CREATE TABLE [dbo].[Shippers]([ShipperID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, [CompanyName] [nvarchar](40) NOT NULL, [Phone] [nvarchar](24) NULL);
                CREATE TABLE [dbo].[Products]([ProductID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, [ProductName] [nvarchar](40) NOT NULL, [SupplierID] [int] NULL, [CategoryID] [int] NULL, [QuantityPerUnit] [nvarchar](20) NULL, [UnitPrice] [money] NULL DEFAULT 0, [UnitsInStock] [smallint] NULL DEFAULT 0, [UnitsOnOrder] [smallint] NULL DEFAULT 0, [ReorderLevel] [smallint] NULL DEFAULT 0, [Discontinued] [bit] NOT NULL DEFAULT 0);
                CREATE TABLE [dbo].[Orders]([OrderID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, [CustomerID] [nchar](5) NULL, [EmployeeID] [int] NULL, [OrderDate] [datetime] NULL, [RequiredDate] [datetime] NULL, [ShippedDate] [datetime] NULL, [ShipVia] [int] NULL, [Freight] [money] NULL DEFAULT 0, [ShipName] [nvarchar](40) NULL, [ShipAddress] [nvarchar](60) NULL, [ShipCity] [nvarchar](15) NULL, [ShipRegion] [nvarchar](15) NULL, [ShipPostalCode] [nvarchar](10) NULL, [ShipCountry] [nvarchar](15) NULL);
                CREATE TABLE [dbo].[Order Details]([OrderID] [int] NOT NULL, [ProductID] [int] NOT NULL, [UnitPrice] [money] NOT NULL DEFAULT 0, [Quantity] [smallint] NOT NULL DEFAULT 1, [Discount] [real] NOT NULL DEFAULT 0, CONSTRAINT [PK_Order_Details] PRIMARY KEY ([OrderID], [ProductID]));
                CREATE TABLE [dbo].[Region]([RegionID] [int] NOT NULL PRIMARY KEY, [RegionDescription] [nchar](50) NOT NULL);
                CREATE TABLE [dbo].[Territories]([TerritoryID] [nvarchar](20) NOT NULL PRIMARY KEY, [TerritoryDescription] [nchar](50) NOT NULL, [RegionID] [int] NOT NULL);
                CREATE TABLE [dbo].[EmployeeTerritories]([EmployeeID] [int] NOT NULL, [TerritoryID] [nvarchar](20) NOT NULL, CONSTRAINT [PK_EmployeeTerritories] PRIMARY KEY ([EmployeeID], [TerritoryID]));
                CREATE TABLE [dbo].[CustomerDemographics]([CustomerTypeID] [nchar](10) NOT NULL PRIMARY KEY, [CustomerDesc] [ntext] NULL);
                CREATE TABLE [dbo].[CustomerCustomerDemo]([CustomerID] [nchar](5) NOT NULL, [CustomerTypeID] [nchar](10) NOT NULL, CONSTRAINT [PK_CustomerCustomerDemo] PRIMARY KEY ([CustomerID], [CustomerTypeID]));
            ");

            var sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "InertNorthwind.sql");

            if (!File.Exists(sqlFile))
            {
                throw new FileNotFoundException($"Không tìm thấy file: {sqlFile}");
            }

            var sqlScript = File.ReadAllText(sqlFile, System.Text.Encoding.UTF8);

            var sqlCommands = Regex.Split(sqlScript, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (var command in sqlCommands)
            {
                var cleanCommand = command.Trim();
                if (!string.IsNullOrWhiteSpace(cleanCommand) && !cleanCommand.StartsWith("USE ", StringComparison.OrdinalIgnoreCase))
                {
                    // Thêm SET QUOTED_IDENTIFIER để tránh lỗi ngoặc kép ""
                    migrationBuilder.Sql("SET QUOTED_IDENTIFIER ON; \n" + cleanCommand);
                }
            }

            // --- 2. TẠO CÁC VIEW (TÁCH RIÊNG TỪNG LỆNH) ---
            migrationBuilder.Sql("CREATE VIEW [dbo].[DanhSachDonHang] AS SELECT o.OrderID, c.ContactName, SUM(od.Quantity) AS Quantity, SUM(od.UnitPrice * od.Quantity) AS TotalPrice,o.OrderDate FROM dbo.Orders AS o INNER JOIN dbo.[Order Details] AS od ON o.OrderID = od.OrderID INNER JOIN dbo.Customers AS c ON o.CustomerID = c.CustomerID GROUP BY o.OrderID, c.ContactName,o.OrderDate;");
            migrationBuilder.Sql("CREATE VIEW [dbo].[DanhSachNhanVien] AS SELECT EmployeeID,LastName,FirstName,Address,City,Country,HomePhone,Username,Password FROM Employees;");
            migrationBuilder.Sql("CREATE VIEW [dbo].[DanhSachSanPham] AS SELECT pr.ProductID, pr.ProductName, pr.UnitPrice, pr.QuantityPerUnit, cata.CategoryName, sup.CompanyName, pr.UnitsInStock FROM Products pr LEFT JOIN Suppliers sup ON pr.SupplierID = sup.SupplierID LEFT JOIN Categories cata ON pr.CategoryID = cata.CategoryID;");
            migrationBuilder.Sql("CREATE VIEW [dbo].[DanhSachKhachHang] AS SELECT CustomerID, ContactName, Address, City, Country, Phone,Username,Password FROM Customers;");
            migrationBuilder.Sql("CREATE VIEW [dbo].[ChiTietDonHang] AS SELECT od.OrderID,c.ContactName,od.UnitPrice,od.Quantity,(od.UnitPrice * od.Quantity) AS TotalPrice FROM [Order Details] od INNER JOIN Orders o ON od.OrderID = o.OrderID INNER JOIN Customers c ON o.CustomerID = c.CustomerID;");
            migrationBuilder.Sql("CREATE VIEW [dbo].[Order Subtotals] AS SELECT OrderID, Sum(CONVERT(money,(UnitPrice*Quantity*(1-Discount)/100))*100) AS Subtotal FROM [Order Details] GROUP BY OrderID;");
            migrationBuilder.Sql("CREATE VIEW [dbo].[Invoices] AS SELECT Orders.ShipName, Orders.CustomerID, Customers.CompanyName AS CustomerName, Orders.OrderID, Products.ProductName, [Order Details].UnitPrice, [Order Details].Quantity FROM Shippers INNER JOIN (Products INNER JOIN ((Employees INNER JOIN (Customers INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID) ON Employees.EmployeeID = Orders.EmployeeID) INNER JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID) ON Products.ProductID = [Order Details].ProductID) ON Shippers.ShipperID = Orders.ShipVia;");

            // --- 3. TẠO STORED PROCEDURES (TÁCH RIÊNG TỪNG LỆNH) ---
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[CustOrderHist] @CustomerID nchar(5) AS SELECT ProductName, Total=SUM(Quantity) FROM Products P, [Order Details] OD, Orders O, Customers C WHERE C.CustomerID = @CustomerID AND C.CustomerID = O.CustomerID AND O.OrderID = OD.OrderID AND OD.ProductID = P.ProductID GROUP BY ProductName;");
            migrationBuilder.Sql(@"CREATE PROC [dbo].[LayDonHangTheoId] (@OrderId int) AS BEGIN SELECT od.OrderID, c.ContactName, p.ProductName,od.UnitPrice,od.Quantity,(od.UnitPrice * od.Quantity) AS [Total Price],c.Address FROM [Order Details] od INNER JOIN Orders o ON od.OrderID = o.OrderID INNER JOIN Products p ON od.ProductID = p.ProductID INNER JOIN Customers c ON o.CustomerID = c.CustomerID WHERE od.OrderID = @OrderId END;");
            migrationBuilder.Sql(@"CREATE PROC [dbo].[LayKhachHangTheoId] (@CustomerID nchar(5)) AS BEGIN SELECT CustomerID, ContactName, Address, City, Country, Phone,UserName,Password FROM Customers WHERE CustomerID = @CustomerID END;");
            migrationBuilder.Sql(@"CREATE PROC [dbo].[LayNhanVienTheoId] (@EmployeeID int) AS BEGIN SELECT EmployeeID,LastName,FirstName,Address,City,Country,HomePhone,Username,Password FROM Employees WHERE EmployeeID = @EmployeeID END;");
            migrationBuilder.Sql(@"
                                CREATE   PROC [dbo].[ThemKhachHang] 
                (@CustomerID nchar(5), @CompanyName nvarchar(40), @ContactName nvarchar(40), @Address nvarchar(60), @City nvarchar(15), @Country nvarchar(15), @Phone nvarchar(24), @Username nvarchar(50),@Password nvarchar(255))      
                AS BEGIN
                    INSERT INTO Customers(CustomerID,CompanyName,ContactName, Address, City, Country, Phone,Username,Password)
                    VALUES (@CustomerID,@CompanyName,@ContactName, @Address, @City, @Country, @Phone,@Username,@Password)
                END
                ");
            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[ThemNhanVien]
( 
	@LastName nvarchar(20),
	@FirstName nvarchar(10),
	@Address nvarchar(60),
	@City nvarchar(15),
	@Country nvarchar(15),
	@HomePhone nvarchar(24),
	@Username nvarchar(50),
	@Password nvarchar(255)
)
AS
BEGIN
	INSERT INTO Employees(LastName,FirstName,Address,City,Country,HomePhone,Username,Password)
	VALUES(@LastName,@FirstName,@Address,@City,@Country,@HomePhone,@Username,@Password)
END
GO

            ");
            migrationBuilder.Sql(@"
                    CREATE   PROC [dbo].[XoaKhachHang] (@CustomerID nvarchar(450))
                AS BEGIN
                    DELETE FROM Customer WHERE CustomerId = @CustomerID
                END
           
            ");
            migrationBuilder.Sql(@"
                    CREATE PROC [dbo].[XoaNhanVien]
(
	@EmployeeID int
)
AS
BEGIN
	DELETE FROM Employees
	WHERE EmployeeID = @EmployeeID
END
            ");
            migrationBuilder.Sql(@"
                    CREATE PROC [dbo].[SuaNhanVien]
(
	@EmployeeID int,
	@LastName nvarchar(20),
	@FirstName nvarchar(10),
	@Address nvarchar(60),
	@City nvarchar(15),
	@Country nvarchar(15),
	@HomePhone nvarchar(24),
	@Username nvarchar(50),
	@Password nvarchar(255)
)
AS
BEGIN
	UPDATE Employees
	SET LastName = @LastName, FirstName = @FirstName, Address = @Address, City = @City, Country = @Country, HomePhone = @HomePhone, Username = @Username, Password = @Password
	WHERE EmployeeID = @EmployeeID
END
GO
            ");
            migrationBuilder.Sql(@"
 CREATE   PROC [dbo].[SuaKhachHang]
                (@CustomerID nchar(5),@CompanyName nvarchar(40),@ContactName nvarchar(40), @Address nvarchar(60), @City nvarchar(15), @Country nvarchar(15), @Phone nvarchar(24),@Username nvarchar(50), @Password nvarchar(255))
                AS BEGIN
                    UPDATE Customers
                    SET CompanyName = @CompanyName,ContactName = @ContactName, Address = @Address, City = @City, Country = @Country, Phone = @Phone,Username = @Username, Password = @Password
                    WHERE CustomerId = @CustomerID
                END
                ");
            migrationBuilder.Sql(@"CREATE PROCEDURE [dbo].[LaySanPhamTheoId] (@ProductID INT) AS BEGIN SELECT ProductID, ProductName, UnitPrice, QuantityPerUnit, CategoryID, SupplierID FROM Products WHERE ProductID = @ProductID END;");
            migrationBuilder.Sql(@"CREATE PROC [dbo].[TimKiemSanPham] (@ProductName nvarchar(40)) AS BEGIN SELECT * FROM DanhSachSanPham WHERE ProductName LIKE '%' + @ProductName + '%' END;");
            migrationBuilder.Sql(@"CREATE PROC [dbo].[ThemSanPham] (@ProductName nvarchar(40), @UnitPrice money, @QuantityPerUnit nvarchar(20), @CategoryID int, @SupplierID int, @UnitsInStock smallint) AS BEGIN INSERT INTO Products(ProductName, UnitPrice, QuantityPerUnit, CategoryID, SupplierID,UnitsInStock) VALUES(@ProductName, @UnitPrice, @QuantityPerUnit, @CategoryID, @SupplierID,@UnitsInStock) END;");
            migrationBuilder.Sql(@"CREATE PROC [dbo].[SuaSanPham] (@ProductID int, @ProductName nvarchar(40), @UnitPrice money, @QuantityPerUnit nvarchar(20), @CategoryID int, @SupplierID int,@UnitsInStock smallint) AS BEGIN UPDATE Products SET ProductName = @ProductName, UnitPrice = @UnitPrice, QuantityPerUnit = @QuantityPerUnit, CategoryID = @CategoryID, SupplierID = @SupplierID, UnitsInStock = @UnitsInStock WHERE ProductID = @ProductID END;");
            migrationBuilder.Sql(@"CREATE PROC [dbo].[XoaSanPham] (@ProductID int) AS BEGIN DELETE FROM Products WHERE ProductID = @ProductID END;");

            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[HoaDonKhachHang]
(
	@CustomerID nchar(5)
)
AS
SELECT o.OrderID, p.ProductName, p.UnitPrice,SUM(od.Quantity) AS Quantity, SUM(od.UnitPrice * od.Quantity) AS [Total Price],c.Address
FROM Orders o
INNER JOIN Customers c ON o.CustomerID = c.CustomerID
INNER JOIN [Order Details] od ON o.OrderID = od.OrderID
INNER JOIN Products p ON od.ProductID = p.ProductID
WHERE o.CustomerID = @CustomerID
GROUP BY o.OrderID,p.ProductName,p.UnitPrice,c.Address


        ");


            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[sp_DeleteOrder_RestoreStock] @OrderID INT AS 
                BEGIN 
                    SET NOCOUNT ON; 
                    BEGIN TRY 
                        BEGIN TRAN; 
                        IF NOT EXISTS (SELECT 1 FROM dbo.Orders WHERE OrderID = @OrderID) THROW 50004, N'Đơn hàng không tồn tại', 1; 
                        UPDATE p SET p.UnitsInStock = p.UnitsInStock + od.Quantity FROM dbo.Products p JOIN dbo.[Order Details] od ON p.ProductID = od.ProductID WHERE od.OrderID = @OrderID; 
                        DELETE FROM dbo.[Order Details] WHERE OrderID = @OrderID; 
                        DELETE FROM dbo.Orders WHERE OrderID = @OrderID; 
                        COMMIT; 
                    END TRY 
                    BEGIN CATCH IF @@TRANCOUNT > 0 ROLLBACK; THROW; END CATCH 
                END;");

            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_CheckStock
                ON dbo.[Order Details]
                INSTEAD OF INSERT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    IF EXISTS (
                        SELECT 1
                        FROM inserted i
                        JOIN dbo.Products p 
                            ON p.ProductID = i.ProductID
                        WHERE i.Quantity > p.UnitsInStock
                    )
                        THROW 50003, N'Không đủ hàng trong kho', 1;

                    INSERT INTO dbo.[Order Details]
                    SELECT * FROM inserted;

                    UPDATE p
                    SET p.UnitsInStock = p.UnitsInStock - i.Quantity
                    FROM dbo.Products p
                    JOIN inserted i 
                        ON p.ProductID = i.ProductID;
                END;
            ");

            // --- 4. RÀNG BUỘC KHÓA NGOẠI (FOREIGN KEYS) ---
            migrationBuilder.Sql(@"
                ALTER TABLE [dbo].[Products] WITH NOCHECK ADD CONSTRAINT [FK_Products_Categories] FOREIGN KEY([CategoryID]) REFERENCES [dbo].[Categories] ([CategoryID]);
                ALTER TABLE [dbo].[Products] WITH NOCHECK ADD CONSTRAINT [FK_Products_Suppliers] FOREIGN KEY([SupplierID]) REFERENCES [dbo].[Suppliers] ([SupplierID]);
                ALTER TABLE [dbo].[Orders] WITH NOCHECK ADD CONSTRAINT [FK_Orders_Customers] FOREIGN KEY([CustomerID]) REFERENCES [dbo].[Customers] ([CustomerID]);
                ALTER TABLE [dbo].[Orders] WITH NOCHECK ADD CONSTRAINT [FK_Orders_Employees] FOREIGN KEY([EmployeeID]) REFERENCES [dbo].[Employees] ([EmployeeID]);
                ALTER TABLE [dbo].[Order Details] WITH NOCHECK ADD CONSTRAINT [FK_Order_Details_Orders] FOREIGN KEY([OrderID]) REFERENCES [dbo].[Orders] ([OrderID]);
                ALTER TABLE [dbo].[Order Details] WITH NOCHECK ADD CONSTRAINT [FK_Order_Details_Products] FOREIGN KEY([ProductID]) REFERENCES [dbo].[Products] ([ProductID]);
                ALTER TABLE [dbo].[Employees] WITH NOCHECK ADD CONSTRAINT [FK_Employees_Employees] FOREIGN KEY([ReportsTo]) REFERENCES [dbo].[Employees] ([EmployeeID]);
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //Drop Triggers
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_CheckStock;");

            // Drop Procedures
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_DeleteOrder_RestoreStock];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[XoaSanPham];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[SuaSanPham];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[ThemSanPham];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[TimKiemSanPham];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[LaySanPhamTheoId];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[SuaKhachHang];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[SuaNhanVien];");   
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[XoaNhanVien];");   
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[XoaKhachHang];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[ThemNhanVien];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[ThemKhachHang];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[LayNhanVienTheoId];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[LayKhachHangTheoId];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[LayDonHangTheoId];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[HoaDonKhachHang];");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[CustOrderHist];");

            // Drop Views
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[Invoices];");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[Order Subtotals];");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[ChiTietDonHang];");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[DanhSachKhachHang];");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[DanhSachSanPham];");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[DanhSachNhanVien];");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[DanhSachDonHang];");


            // Tầng 1: Các bảng giao thoa/nhiều khóa ngoại nhất
            migrationBuilder.DropTable(name: "Order Details");
            migrationBuilder.DropTable(name: "CustomerCustomerDemo");
            migrationBuilder.DropTable(name: "EmployeeTerritories");

            // Tầng 2: Các bảng trung gian
            migrationBuilder.DropTable(name: "Orders");
            migrationBuilder.DropTable(name: "Products");
            migrationBuilder.DropTable(name: "Territories");

            // Tầng 3: Các bảng gốc (Master data)
            migrationBuilder.DropTable(name: "Employees");
            migrationBuilder.DropTable(name: "Customers");
            migrationBuilder.DropTable(name: "Categories");
            migrationBuilder.DropTable(name: "Suppliers");
            migrationBuilder.DropTable(name: "Shippers");
            migrationBuilder.DropTable(name: "Region");
            migrationBuilder.DropTable(name: "CustomerDemographics");

        }
    }
}