using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourProject.Migrations
{
    public partial class FullNorthwindMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // --- 1. TẠO TẤT CẢ CÁC BẢNG (14 TABLES) ---
            migrationBuilder.Sql(@"
                CREATE TABLE [dbo].[Categories]([CategoryID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, [CategoryName] [nvarchar](15) NOT NULL, [Description] [ntext] NULL, [Picture] [image] NULL);
                CREATE TABLE [dbo].[Suppliers]([SupplierID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, [CompanyName] [nvarchar](40) NOT NULL, [ContactName] [nvarchar](30) NULL, [ContactTitle] [nvarchar](30) NULL, [Address] [nvarchar](60) NULL, [City] [nvarchar](15) NULL, [Region] [nvarchar](15) NULL, [PostalCode] [nvarchar](10) NULL, [Country] [nvarchar](15) NULL, [Phone] [nvarchar](24) NULL, [Fax] [nvarchar](24) NULL, [HomePage] [ntext] NULL);
                CREATE TABLE [dbo].[Shippers]([ShipperID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, [CompanyName] [nvarchar](40) NOT NULL, [Phone] [nvarchar](24) NULL);
                CREATE TABLE [dbo].[Region]([RegionID] [int] NOT NULL PRIMARY KEY, [RegionDescription] [nchar](50) NOT NULL);
                CREATE TABLE [dbo].[Territories]([TerritoryID] [nvarchar](20) NOT NULL PRIMARY KEY, [TerritoryDescription] [nchar](50) NOT NULL, [RegionID] [int] NOT NULL);
                CREATE TABLE [dbo].[Customers]([CustomerID] [nchar](5) NOT NULL PRIMARY KEY, [CompanyName] [nvarchar](40) NOT NULL, [ContactName] [nvarchar](30) NULL, [ContactTitle] [nvarchar](30) NULL, [Address] [nvarchar](60) NULL, [City] [nvarchar](15) NULL, [Region] [nvarchar](15) NULL, [PostalCode] [nvarchar](10) NULL, [Country] [nvarchar](15) NULL, [Phone] [nvarchar](24) NULL, [Fax] [nvarchar](24) NULL, [Username] [nvarchar](50) NULL, [Password] [nvarchar](50) NULL);
                CREATE TABLE [dbo].[Employees]([EmployeeID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, [LastName] [nvarchar](20) NOT NULL, [FirstName] [nvarchar](10) NOT NULL, [Title] [nvarchar](30) NULL, [TitleOfCourtesy] [nvarchar](25) NULL, [BirthDate] [datetime] NULL, [HireDate] [datetime] NULL, [Address] [nvarchar](60) NULL, [City] [nvarchar](15) NULL, [Region] [nvarchar](15) NULL, [PostalCode] [nvarchar](10) NULL, [Country] [nvarchar](15) NULL, [HomePhone] [nvarchar](24) NULL, [Extension] [nvarchar](4) NULL, [Photo] [image] NULL, [Notes] [ntext] NULL, [ReportsTo] [int] NULL, [PhotoPath] [nvarchar](255) NULL, [Username] [nvarchar](50) NULL, [Password] [nvarchar](50) NULL);
                CREATE TABLE [dbo].[EmployeeTerritories]([EmployeeID] [int] NOT NULL, [TerritoryID] [nvarchar](20) NOT NULL, CONSTRAINT [PK_EmployeeTerritories] PRIMARY KEY ([EmployeeID], [TerritoryID]));
                CREATE TABLE [dbo].[Products]([ProductID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, [ProductName] [nvarchar](40) NOT NULL, [SupplierID] [int] NULL, [CategoryID] [int] NULL, [QuantityPerUnit] [nvarchar](20) NULL, [UnitPrice] [money] DEFAULT (0), [UnitsInStock] [smallint] DEFAULT (0), [UnitsOnOrder] [smallint] DEFAULT (0), [ReorderLevel] [smallint] DEFAULT (0), [Discontinued] [bit] NOT NULL DEFAULT (0));
                CREATE TABLE [dbo].[Orders]([OrderID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY, [CustomerID] [nchar](5) NULL, [EmployeeID] [int] NULL, [OrderDate] [datetime] NULL, [RequiredDate] [datetime] NULL, [ShippedDate] [datetime] NULL, [ShipVia] [int] NULL, [Freight] [money] DEFAULT (0), [ShipName] [nvarchar](40) NULL, [ShipAddress] [nvarchar](60) NULL, [ShipCity] [nvarchar](15) NULL, [ShipRegion] [nvarchar](15) NULL, [ShipPostalCode] [nvarchar](10) NULL, [ShipCountry] [nvarchar](15) NULL);
                CREATE TABLE [dbo].[Order Details]([OrderID] [int] NOT NULL, [ProductID] [int] NOT NULL, [UnitPrice] [money] NOT NULL DEFAULT (0), [Quantity] [smallint] NOT NULL DEFAULT (1), [Discount] [real] NOT NULL DEFAULT (0), CONSTRAINT [PK_Order_Details] PRIMARY KEY ([OrderID], [ProductID]));
                CREATE TABLE [dbo].[CustomerDemographics]([CustomerTypeID] [nchar](10) NOT NULL PRIMARY KEY, [CustomerDesc] [ntext] NULL);
                CREATE TABLE [dbo].[CustomerCustomerDemo]([CustomerID] [nchar](5) NOT NULL, [CustomerTypeID] [nchar](10) NOT NULL, CONSTRAINT [PK_CustomerCustomerDemo] PRIMARY KEY ([CustomerID], [CustomerTypeID]));
                CREATE TABLE [dbo].[__EFMigrationsHistory]([MigrationId] [nvarchar](150) NOT NULL PRIMARY KEY, [ProductVersion] [nvarchar](32) NOT NULL);
            ");

            // --- 2. TẠO TẤT CẢ CÁC VIEWS (21 VIEWS) ---
            migrationBuilder.Sql(@"
                CREATE VIEW [dbo].[DanhSachDonHang] AS SELECT o.OrderID, c.ContactName, SUM(od.Quantity) AS Quantity, SUM(od.UnitPrice * od.Quantity) AS TotalPrice FROM dbo.Orders AS o INNER JOIN dbo.[Order Details] AS od ON o.OrderID = od.OrderID INNER JOIN dbo.Customers AS c ON o.CustomerID = c.CustomerID GROUP BY o.OrderID, c.ContactName;
                GO
                CREATE VIEW [dbo].[DanhSachNhanVien] AS SELECT EmployeeID,LastName,FirstName,Address,City,Country,HomePhone,Username,Password FROM Employees;
                GO
                CREATE VIEW [dbo].[DanhSachSanPham] AS SELECT pr.ProductID, pr.ProductName, pr.UnitPrice, pr.QuantityPerUnit, cata.CategoryName, sup.CompanyName FROM Products pr LEFT JOIN Suppliers sup ON pr.SupplierID = sup.SupplierID LEFT JOIN Categories cata ON pr.CategoryID = cata.CategoryID;
                GO
                CREATE VIEW [dbo].[DanhSachKhachHang] AS SELECT CustomerID, ContactName, Address, City, Country, Phone,Username,Password FROM Customers;
                GO
                CREATE VIEW [dbo].[Customer and Suppliers by City] AS SELECT City, CompanyName, ContactName, 'Customers' AS Relationship FROM Customers UNION SELECT City, CompanyName, ContactName, 'Suppliers' FROM Suppliers;
                GO
                CREATE VIEW [dbo].[Alphabetical list of products] AS SELECT Products.*, Categories.CategoryName FROM Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID WHERE (((Products.Discontinued)=0));
                GO
                CREATE VIEW [dbo].[Current Product List] AS SELECT Product_List.ProductID, Product_List.ProductName FROM Products AS Product_List WHERE (((Product_List.Discontinued)=0));
                GO
                CREATE VIEW [dbo].[Orders Qry] AS SELECT Orders.OrderID, Orders.CustomerID, Orders.EmployeeID, Orders.OrderDate, Orders.RequiredDate, Orders.ShippedDate, Orders.ShipVia, Orders.Freight, Orders.ShipName, Orders.ShipAddress, Orders.ShipCity, Orders.ShipRegion, Orders.ShipPostalCode, Orders.ShipCountry, Customers.CompanyName, Customers.Address, Customers.City, Customers.Region, Customers.PostalCode, Customers.Country FROM Customers INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID;
                GO
                CREATE VIEW [dbo].[Products Above Average Price] AS SELECT Products.ProductName, Products.UnitPrice FROM Products WHERE Products.UnitPrice>(SELECT AVG(UnitPrice) From Products);
                GO
                CREATE VIEW [dbo].[Products by Category] AS SELECT Categories.CategoryName, Products.ProductName, Products.QuantityPerUnit, Products.UnitsInStock, Products.Discontinued FROM Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID WHERE Products.Discontinued <> 1;
                GO
                CREATE VIEW [dbo].[Quarterly Orders] AS SELECT DISTINCT Customers.CustomerID, Customers.CompanyName, Customers.City, Customers.Country FROM Customers RIGHT JOIN Orders ON Customers.CustomerID = Orders.CustomerID WHERE Orders.OrderDate BETWEEN '19970101' And '19971231';
                GO
                CREATE VIEW [dbo].[Order Details Extended] AS SELECT [Order Details].OrderID, [Order Details].ProductID, Products.ProductName, [Order Details].UnitPrice, [Order Details].Quantity, [Order Details].Discount, (CONVERT(money,([Order Details].UnitPrice*Quantity*(1-Discount)/100))*100) AS ExtendedPrice FROM Products INNER JOIN [Order Details] ON Products.ProductID = [Order Details].ProductID;
                GO
                CREATE VIEW [dbo].[Order Subtotals] AS SELECT [Order Details].OrderID, Sum(CONVERT(money,([Order Details].UnitPrice*Quantity*(1-Discount)/100))*100) AS Subtotal FROM [Order Details] GROUP BY [Order Details].OrderID;
                GO
                CREATE VIEW [dbo].[Invoices] AS SELECT Orders.ShipName, Orders.ShipAddress, Orders.ShipCity, Orders.ShipRegion, Orders.ShipPostalCode, Orders.ShipCountry, Orders.CustomerID, Customers.CompanyName AS CustomerName, Customers.Address, Customers.City, Customers.Region, Customers.PostalCode, Customers.Country, (FirstName + ' ' + LastName) AS Salesperson, Orders.OrderID, Orders.OrderDate, Orders.RequiredDate, Orders.ShippedDate, Shippers.CompanyName As ShipperName, [Order Details].ProductID, Products.ProductName, [Order Details].UnitPrice, [Order Details].Quantity, [Order Details].Discount, (CONVERT(money,([Order Details].UnitPrice*Quantity*(1-Discount)/100))*100) AS ExtendedPrice, Orders.Freight FROM Shippers INNER JOIN (Products INNER JOIN ((Employees INNER JOIN (Customers INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID) ON Employees.EmployeeID = Orders.EmployeeID) INNER JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID) ON Products.ProductID = [Order Details].ProductID) ON Shippers.ShipperID = Orders.ShipVia;
                GO
                CREATE VIEW [dbo].[Product Sales for 1997] AS SELECT Categories.CategoryName, Products.ProductName, Sum(CONVERT(money,([Order Details].UnitPrice*Quantity*(1-Discount)/100))*100) AS ProductSales FROM (Categories INNER JOIN Products ON Categories.CategoryID = Products.CategoryID) INNER JOIN (Orders INNER JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID) ON Products.ProductID = [Order Details].ProductID WHERE (((Orders.ShippedDate) Between '19970101' And '19971231')) GROUP BY Categories.CategoryName, Products.ProductName;
                GO
                CREATE VIEW [dbo].[Category Sales for 1997] AS SELECT [Product Sales for 1997].CategoryName, Sum([Product Sales for 1997].ProductSales) AS CategorySales FROM [Product Sales for 1997] GROUP BY [Product Sales for 1997].CategoryName;
                GO
                CREATE VIEW [dbo].[Sales by Category] AS SELECT Categories.CategoryID, Categories.CategoryName, Products.ProductName, Sum([Order Details Extended].ExtendedPrice) AS ProductSales FROM Categories INNER JOIN (Products INNER JOIN (Orders INNER JOIN [Order Details Extended] ON Orders.OrderID = [Order Details Extended].OrderID) ON Products.ProductID = [Order Details Extended].ProductID) ON Categories.CategoryID = Products.CategoryID WHERE Orders.OrderDate BETWEEN '19970101' And '19971231' GROUP BY Categories.CategoryID, Categories.CategoryName, Products.ProductName;
                GO
                CREATE VIEW [dbo].[Sales Totals by Amount] AS SELECT [Order Subtotals].Subtotal AS SaleAmount, Orders.OrderID, Customers.CompanyName, Orders.ShippedDate FROM Customers INNER JOIN (Orders INNER JOIN [Order Subtotals] ON Orders.OrderID = [Order Subtotals].OrderID) ON Customers.CustomerID = Orders.CustomerID WHERE ([Order Subtotals].Subtotal >2500) AND (Orders.ShippedDate BETWEEN '19970101' And '19971231');
                GO
                CREATE VIEW [dbo].[Summary of Sales by Quarter] AS SELECT Orders.ShippedDate, Orders.OrderID, [Order Subtotals].Subtotal FROM Orders INNER JOIN [Order Subtotals] ON Orders.OrderID = [Order Subtotals].OrderID WHERE Orders.ShippedDate IS NOT NULL;
                GO
                CREATE VIEW [dbo].[Summary of Sales by Year] AS SELECT Orders.ShippedDate, Orders.OrderID, [Order Subtotals].Subtotal FROM Orders INNER JOIN [Order Subtotals] ON Orders.OrderID = [Order Subtotals].OrderID WHERE Orders.ShippedDate IS NOT NULL;
                GO
                CREATE VIEW [dbo].[ChiTietDonHang] AS SELECT od.OrderID,c.ContactName,od.UnitPrice,od.Quantity,(od.UnitPrice * od.Quantity) AS TotalPrice FROM [Order Details] od INNER JOIN Orders o ON od.OrderID = o.OrderID INNER JOIN Customers c ON o.CustomerID = c.CustomerID;
            ".Replace("GO", ""));

            // --- 3. TẠO TẤT CẢ CÁC STORED PROCEDURES (25 PROCEDURES) ---
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[CustOrderHist] @CustomerID nchar(5) AS SELECT ProductName, Total=SUM(Quantity) FROM Products P, [Order Details] OD, Orders O, Customers C WHERE C.CustomerID = @CustomerID AND C.CustomerID = O.CustomerID AND O.OrderID = OD.OrderID AND OD.ProductID = P.ProductID GROUP BY ProductName;
            ");
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[CustOrdersDetail] @OrderID int AS SELECT ProductName, UnitPrice=ROUND(Od.UnitPrice, 2), Quantity, Discount=CONVERT(int, Discount * 100), ExtendedPrice=ROUND(CONVERT(money, Quantity * (1 - Discount) * Od.UnitPrice), 2) FROM Products P, [Order Details] Od WHERE Od.ProductID = P.ProductID and Od.OrderID = @OrderID;
            ");
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[CustOrdersOrders] @CustomerID nchar(5) AS SELECT OrderID, OrderDate, RequiredDate, ShippedDate FROM Orders WHERE CustomerID = @CustomerID ORDER BY OrderID;
            ");
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[Employee Sales by Country] @Beginning_Date DateTime, @Ending_Date DateTime AS SELECT Employees.Country, Employees.LastName, Employees.FirstName, Orders.ShippedDate, Orders.OrderID, [Order Subtotals].Subtotal AS SaleAmount FROM Employees INNER JOIN (Orders INNER JOIN [Order Subtotals] ON Orders.OrderID = [Order Subtotals].OrderID) ON Employees.EmployeeID = Orders.EmployeeID WHERE Orders.ShippedDate Between @Beginning_Date And @Ending_Date;
            ");
            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[HoaDonKhachHang] (@CustomerID nchar(5)) AS SELECT o.OrderID, p.ProductName, p.UnitPrice,SUM(od.Quantity) AS Quantity, SUM(od.UnitPrice * od.Quantity) AS [Total Price] FROM Orders o INNER JOIN Customers c ON o.CustomerID = c.CustomerID INNER JOIN [Order Details] od ON o.OrderID = od.OrderID INNER JOIN Products p ON od.ProductID = p.ProductID WHERE o.CustomerID = @CustomerID GROUP BY o.OrderID,p.ProductName,p.UnitPrice;
            ");
            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[LayDonHangTheoId] (@OrderId int) AS BEGIN SELECT od.OrderID, c.ContactName, p.ProductName,od.UnitPrice,od.Quantity,(od.UnitPrice * od.Quantity) AS [Total Price] FROM [Order Details] od INNER JOIN Orders o ON od.OrderID = o.OrderID INNER JOIN Products p ON od.ProductID = p.ProductID INNER JOIN Customers c ON o.CustomerID = c.CustomerID WHERE od.OrderID = @OrderId END;
            ");
            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[LayKhachHangTheoId] (@CustomerID nchar(5)) AS BEGIN SELECT CustomerID, ContactName, Address, City, Country, Phone,UserName,Password FROM Customers WHERE CustomerID = @CustomerID END;
            ");
            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[LayNhanVienTheoId] (@EmployeeID int) AS BEGIN SELECT EmployeeID,LastName,FirstName,Address,City,Country,HomePhone,Username,Password FROM Employees WHERE EmployeeID = @EmployeeID END;
            ");
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[LaySanPhamTheoId] (@ProductID INT) AS BEGIN SELECT ProductID, ProductName, UnitPrice, QuantityPerUnit, CategoryID, SupplierID FROM Products WHERE ProductID = @ProductID END;
            ");
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[Sales by Year] @Beginning_Date DateTime, @Ending_Date DateTime AS SELECT Orders.ShippedDate, Orders.OrderID, [Order Subtotals].Subtotal, DATENAME(yy,ShippedDate) AS Year FROM Orders INNER JOIN [Order Subtotals] ON Orders.OrderID = [Order Subtotals].OrderID WHERE Orders.ShippedDate Between @Beginning_Date And @Ending_Date;
            ");
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[SalesByCategory] @CategoryName nvarchar(15), @OrdYear nvarchar(4) = '1998' AS IF @OrdYear != '1996' AND @OrdYear != '1997' AND @OrdYear != '1998' BEGIN SELECT @OrdYear = '1998' END SELECT ProductName, TotalPurchase=ROUND(SUM(CONVERT(decimal(14,2), OD.Quantity * (1-OD.Discount) * OD.UnitPrice)), 0) FROM [Order Details] OD, Orders O, Products P, Categories C WHERE OD.OrderID = O.OrderID AND OD.ProductID = P.ProductID AND P.CategoryID = C.CategoryID AND C.CategoryName = @CategoryName AND SUBSTRING(CONVERT(nvarchar(22), O.OrderDate, 111), 1, 4) = @OrdYear GROUP BY ProductName ORDER BY ProductName;
            ");
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[sp_DeleteOrder_RestoreStock] @OrderID INT AS BEGIN SET NOCOUNT ON; BEGIN TRY BEGIN TRAN; IF NOT EXISTS (SELECT 1 FROM dbo.Orders WHERE OrderID = @OrderID) THROW 50004, N'Đơn hàng không tồn tại', 1; UPDATE p SET p.UnitsInStock = p.UnitsInStock + od.Quantity FROM dbo.Products p JOIN dbo.[Order Details] od ON p.ProductID = od.ProductID WHERE od.OrderID = @OrderID; DELETE FROM dbo.[Order Details] WHERE OrderID = @OrderID; DELETE FROM dbo.Orders WHERE OrderID = @OrderID; COMMIT; END TRY BEGIN CATCH IF @@TRANCOUNT > 0 ROLLBACK; THROW; END CATCH END;
            ");
            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[SuaKhachHang] (@CustomerID nchar(5),@CompanyName nvarchar(40),@ContactName nvarchar(40), @Address nvarchar(60), @City nvarchar(15), @Country nvarchar(15), @Phone nvarchar(24)) AS BEGIN UPDATE Customers SET CompanyName = @CompanyName,ContactName = @ContactName, Address = @Address, City = @City, Country = @Country, Phone = @Phone WHERE CustomerId = @CustomerID END;
            ");
            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[SuaNhanVien] (@EmployeeID int, @LastName nvarchar(20), @FirstName nvarchar(10), @Address nvarchar(60), @City nvarchar(15), @Country nvarchar(15), @HomePhone nvarchar(24), @Username nvarchar(50), @Password nvarchar(50)) AS BEGIN UPDATE Employees SET LastName = @LastName, FirstName = @FirstName, Address = @Address, City = @City, Country = @Country, HomePhone = @HomePhone, Username = @Username, Password = @Password WHERE EmployeeID = @EmployeeID END;
            ");
            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[SuaSanPham] (@ProductID int, @ProductName nvarchar(40), @UnitPrice money, @QuantityPerUnit nvarchar(20), @CategoryID int, @SupplierID int) AS BEGIN UPDATE Products SET ProductName = @ProductName, UnitPrice = @UnitPrice, QuantityPerUnit = @QuantityPerUnit, CategoryID = @CategoryID, SupplierID = @SupplierID WHERE ProductID = @ProductID END;
            ");
            migrationBuilder.Sql(@"
                CREATE PROCEDURE [dbo].[Ten Most Expensive Products] AS SET ROWCOUNT 10 SELECT Products.ProductName AS TenMostExpensiveProducts, Products.UnitPrice FROM Products ORDER BY Products.UnitPrice DESC;
            ");
            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[TimKiemSanPham] (@ProductName nvarchar(40)) AS BEGIN SELECT * FROM DanhSachSanPham WHERE ProductName LIKE '%' + @ProductName + '%' END;
            ");
            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[ThemKhachHang] (@CustomerID nchar(5), @CompanyName nvarchar(40), @ContactName nvarchar(40), @Address nvarchar(60), @City nvarchar(15), @Country nvarchar(15), @Phone nvarchar(24), @Username nvarchar(50),@Password nvarchar(50)) AS BEGIN INSERT INTO Customers(CustomerID,CompanyName,ContactName, Address, City, Country, Phone,Username,Password) VALUES (@CustomerID,@CompanyName,@ContactName, @Address, @City, @Country, @Phone,@Username,@Password) END;
            ");
            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[ThemNhanVien] (@LastName nvarchar(20), @FirstName nvarchar(10), @Address nvarchar(60), @City nvarchar(15), @Country nvarchar(15), @HomePhone nvarchar(24), @Username nvarchar(50), @Password nvarchar(50)) AS BEGIN INSERT INTO Employees(LastName,FirstName,Address,City,Country,HomePhone,Username,Password) VALUES(@LastName,@FirstName,@Address,@City,@Country,@HomePhone,@Username,@Password) END;
            ");
            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[ThemSanPham] (@ProductName nvarchar(40), @UnitPrice money, @QuantityPerUnit nvarchar(20), @CategoryID int, @SupplierID int) AS BEGIN INSERT INTO Products(ProductName, UnitPrice, QuantityPerUnit, CategoryID, SupplierID) VALUES(@ProductName, @UnitPrice, @QuantityPerUnit, @CategoryID, @SupplierID) END;
            ");
            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[XemLoaiSanPham] AS BEGIN SELECT CategoryID,CategoryName FROM Categories END;
            ");
            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[XemNhaCungCap] AS BEGIN SELECT SupplierID,CompanyName FROM Suppliers END;
            ");
            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[XoaKhachHang] (@CustomerID nvarchar(450)) AS BEGIN DELETE FROM Customers WHERE CustomerId = @CustomerID END;
            ");
            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[XoaNhanVien] (@EmployeeID int) AS BEGIN DELETE FROM Employees WHERE EmployeeID = @EmployeeID END;
            ");
            migrationBuilder.Sql(@"
                CREATE PROC [dbo].[XoaSanPham] (@ProductID int) AS BEGIN DELETE FROM Products WHERE ProductID = @ProductID END;
            ");

            // --- 4. RÀNG BUỘC KHÓA NGOẠI (FOREIGN KEYS) ---
            migrationBuilder.Sql(@"
                ALTER TABLE [dbo].[Products] WITH NOCHECK ADD CONSTRAINT [FK_Products_Categories] FOREIGN KEY([CategoryID]) REFERENCES [dbo].[Categories] ([CategoryID]);
                ALTER TABLE [dbo].[Products] WITH NOCHECK ADD CONSTRAINT [FK_Products_Suppliers] FOREIGN KEY([SupplierID]) REFERENCES [dbo].[Suppliers] ([SupplierID]);
                ALTER TABLE [dbo].[Orders] WITH NOCHECK ADD CONSTRAINT [FK_Orders_Customers] FOREIGN KEY([CustomerID]) REFERENCES [dbo].[Customers] ([CustomerID]);
                ALTER TABLE [dbo].[Order Details] WITH NOCHECK ADD CONSTRAINT [FK_Order_Details_Orders] FOREIGN KEY([OrderID]) REFERENCES [dbo].[Orders] ([OrderID]);
                ALTER TABLE [dbo].[Order Details] WITH NOCHECK ADD CONSTRAINT [FK_Order_Details_Products] FOREIGN KEY([ProductID]) REFERENCES [dbo].[Products] ([ProductID]);
                ALTER TABLE [dbo].[Employees] WITH NOCHECK ADD CONSTRAINT [FK_Employees_Employees] FOREIGN KEY([ReportsTo]) REFERENCES [dbo].[Employees] ([EmployeeID]);
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop Procedures
            string[] procs = { "CustOrderHist", "CustOrdersDetail", "CustOrdersOrders", "Employee Sales by Country", "HoaDonKhachHang", "LayDonHangTheoId", "LayKhachHangTheoId", "LayNhanVienTheoId", "LaySanPhamTheoId", "Sales by Year", "SalesByCategory", "sp_DeleteOrder_RestoreStock", "SuaKhachHang", "SuaNhanVien", "SuaSanPham", "Ten Most Expensive Products", "TimKiemSanPham", "ThemKhachHang", "ThemNhanVien", "ThemSanPham", "XemLoaiSanPham", "XemNhaCungCap", "XoaKhachHang", "XoaNhanVien", "XoaSanPham" };
            foreach (var proc in procs) migrationBuilder.Sql($"DROP PROCEDURE IF EXISTS [dbo].[{proc}]");

            // Drop Views
            string[] views = { "DanhSachDonHang", "DanhSachNhanVien", "DanhSachSanPham", "DanhSachKhachHang", "Customer and Suppliers by City", "Alphabetical list of products", "Current Product List", "Orders Qry", "Products Above Average Price", "Products by Category", "Quarterly Orders", "Invoices", "Order Details Extended", "Order Subtotals", "Product Sales for 1997", "Category Sales for 1997", "Sales by Category", "Sales Totals by Amount", "Summary of Sales by Quarter", "Summary of Sales by Year", "ChiTietDonHang" };
            foreach (var view in views) migrationBuilder.Sql($"DROP VIEW IF EXISTS [dbo].[{view}]");

            // Drop Tables
            migrationBuilder.DropTable(name: "Order Details");
            migrationBuilder.DropTable(name: "Orders");
            migrationBuilder.DropTable(name: "Products");
            migrationBuilder.DropTable(name: "EmployeeTerritories");
            migrationBuilder.DropTable(name: "Employees");
            migrationBuilder.DropTable(name: "CustomerCustomerDemo");
            migrationBuilder.DropTable(name: "Customers");
            migrationBuilder.DropTable(name: "Suppliers");
            migrationBuilder.DropTable(name: "Categories");
            migrationBuilder.DropTable(name: "Shippers");
            migrationBuilder.DropTable(name: "Territories");
            migrationBuilder.DropTable(name: "Region");
            migrationBuilder.DropTable(name: "CustomerDemographics");
        }
    }
}