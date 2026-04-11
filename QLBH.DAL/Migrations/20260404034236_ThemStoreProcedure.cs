using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLBH.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ThemStoreProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            {
                migrationBuilder.AddColumn<string>(
                    name: "Username",
                    table: "Customer",
                    type: "nvarchar(max)",
                    nullable: true);

                migrationBuilder.AddColumn<string>(
                    name: "Password",
                    table: "Customer",
                    type: "nvarchar(max)",
                    nullable: true);

                // Thêm cột cho bảng Employee
                migrationBuilder.AddColumn<string>(
                    name: "Username",
                    table: "Employee",
                    type: "nvarchar(max)",
                    nullable: true);

                migrationBuilder.AddColumn<string>(
                    name: "Password",
                    table: "Employee",
                    type: "nvarchar(max)",
                    nullable: true);
            }

            //migrationBuilder.CreateTable(
            //    name: "Categories",
            //    columns: table => new
            //    {
            //        CategoryId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Picture = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Categories", x => x.CategoryId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "CategoryReqs",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CategoryReqs", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Customer",
            //    columns: table => new
            //    {
            //        CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        ContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ContactTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        City = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Customer", x => x.CustomerId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "CustomerDemographic",
            //    columns: table => new
            //    {
            //        CustomerTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        CustomerDesc = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CustomerDemographic", x => x.CustomerTypeId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Employee",
            //    columns: table => new
            //    {
            //        EmployeeId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        TitleOfCourtesy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        HireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        City = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        HomePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Photo = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
            //        Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ReportsTo = table.Column<int>(type: "int", nullable: true),
            //        PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ReportsToNavigationEmployeeId = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Employee", x => x.EmployeeId);
            //        table.ForeignKey(
            //            name: "FK_Employee_Employee_ReportsToNavigationEmployeeId",
            //            column: x => x.ReportsToNavigationEmployeeId,
            //            principalTable: "Employee",
            //            principalColumn: "EmployeeId");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Region",
            //    columns: table => new
            //    {
            //        RegionId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        RegionDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Region", x => x.RegionId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Shipper",
            //    columns: table => new
            //    {
            //        ShipperId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Shipper", x => x.ShipperId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "SupplierReqs",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SupplierReqs", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Suppliers",
            //    columns: table => new
            //    {
            //        SupplierId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        ContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ContactTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        City = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        HomePage = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "CustomerCustomerDemographic",
            //    columns: table => new
            //    {
            //        CustomerTypesCustomerTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        CustomersCustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CustomerCustomerDemographic", x => new { x.CustomerTypesCustomerTypeId, x.CustomersCustomerId });
            //        table.ForeignKey(
            //            name: "FK_CustomerCustomerDemographic_CustomerDemographic_CustomerTypesCustomerTypeId",
            //            column: x => x.CustomerTypesCustomerTypeId,
            //            principalTable: "CustomerDemographic",
            //            principalColumn: "CustomerTypeId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_CustomerCustomerDemographic_Customer_CustomersCustomerId",
            //            column: x => x.CustomersCustomerId,
            //            principalTable: "Customer",
            //            principalColumn: "CustomerId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Territory",
            //    columns: table => new
            //    {
            //        TerritoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        TerritoryDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        RegionId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Territory", x => x.TerritoryId);
            //        table.ForeignKey(
            //            name: "FK_Territory_Region_RegionId",
            //            column: x => x.RegionId,
            //            principalTable: "Region",
            //            principalColumn: "RegionId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Order",
            //    columns: table => new
            //    {
            //        OrderId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
            //        EmployeeId = table.Column<int>(type: "int", nullable: true),
            //        OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        RequiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        ShippedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        ShipVia = table.Column<int>(type: "int", nullable: true),
            //        Freight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        ShipName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ShipAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ShipCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ShipRegion = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ShipPostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ShipCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ShipViaNavigationShipperId = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Order", x => x.OrderId);
            //        table.ForeignKey(
            //            name: "FK_Order_Customer_CustomerId",
            //            column: x => x.CustomerId,
            //            principalTable: "Customer",
            //            principalColumn: "CustomerId");
            //        table.ForeignKey(
            //            name: "FK_Order_Employee_EmployeeId",
            //            column: x => x.EmployeeId,
            //            principalTable: "Employee",
            //            principalColumn: "EmployeeId");
            //        table.ForeignKey(
            //            name: "FK_Order_Shipper_ShipViaNavigationShipperId",
            //            column: x => x.ShipViaNavigationShipperId,
            //            principalTable: "Shipper",
            //            principalColumn: "ShipperId");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Products",
            //    columns: table => new
            //    {
            //        ProductId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        SupplierId = table.Column<int>(type: "int", nullable: true),
            //        CategoryId = table.Column<int>(type: "int", nullable: true),
            //        QuantityPerUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
            //        UnitsInStock = table.Column<short>(type: "smallint", nullable: true),
            //        UnitsOnOrder = table.Column<short>(type: "smallint", nullable: true),
            //        ReorderLevel = table.Column<short>(type: "smallint", nullable: true),
            //        Discontinued = table.Column<bool>(type: "bit", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Products", x => x.ProductId);
            //        table.ForeignKey(
            //            name: "FK_Products_Categories_CategoryId",
            //            column: x => x.CategoryId,
            //            principalTable: "Categories",
            //            principalColumn: "CategoryId");
            //        table.ForeignKey(
            //            name: "FK_Products_Suppliers_SupplierId",
            //            column: x => x.SupplierId,
            //            principalTable: "Suppliers",
            //            principalColumn: "SupplierId");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "EmployeeTerritory",
            //    columns: table => new
            //    {
            //        EmployeesEmployeeId = table.Column<int>(type: "int", nullable: false),
            //        TerritoriesTerritoryId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_EmployeeTerritory", x => new { x.EmployeesEmployeeId, x.TerritoriesTerritoryId });
            //        table.ForeignKey(
            //            name: "FK_EmployeeTerritory_Employee_EmployeesEmployeeId",
            //            column: x => x.EmployeesEmployeeId,
            //            principalTable: "Employee",
            //            principalColumn: "EmployeeId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_EmployeeTerritory_Territory_TerritoriesTerritoryId",
            //            column: x => x.TerritoriesTerritoryId,
            //            principalTable: "Territory",
            //            principalColumn: "TerritoryId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "OrderDetail",
            //    columns: table => new
            //    {
            //        OrderId = table.Column<int>(type: "int", nullable: false),
            //        ProductId = table.Column<int>(type: "int", nullable: false),
            //        UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        Quantity = table.Column<short>(type: "smallint", nullable: false),
            //        Discount = table.Column<float>(type: "real", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_OrderDetail", x => new { x.OrderId, x.ProductId });
            //        table.ForeignKey(
            //            name: "FK_OrderDetail_Order_OrderId",
            //            column: x => x.OrderId,
            //            principalTable: "Order",
            //            principalColumn: "OrderId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_OrderDetail_Products_ProductId",
            //            column: x => x.ProductId,
            //            principalTable: "Products",
            //            principalColumn: "ProductId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_CustomerCustomerDemographic_CustomersCustomerId",
            //    table: "CustomerCustomerDemographic",
            //    column: "CustomersCustomerId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Employee_ReportsToNavigationEmployeeId",
            //    table: "Employee",
            //    column: "ReportsToNavigationEmployeeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_EmployeeTerritory_TerritoriesTerritoryId",
            //    table: "EmployeeTerritory",
            //    column: "TerritoriesTerritoryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Order_CustomerId",
            //    table: "Order",
            //    column: "CustomerId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Order_EmployeeId",
            //    table: "Order",
            //    column: "EmployeeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Order_ShipViaNavigationShipperId",
            //    table: "Order",
            //    column: "ShipViaNavigationShipperId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_OrderDetail_ProductId",
            //    table: "OrderDetail",
            //    column: "ProductId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Products_CategoryId",
            //    table: "Products",
            //    column: "CategoryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Products_SupplierId",
            //    table: "Products",
            //    column: "SupplierId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Territory_RegionId",
            //    table: "Territory",
            //    column: "RegionId");

            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[DanhSachSanPham]
                AS
                    SELECT pr.ProductID, pr.ProductName, pr.UnitPrice, pr.QuantityPerUnit, cata.CategoryName, sup.CompanyName
                    FROM Products pr
                    LEFT JOIN Suppliers sup ON pr.SupplierID = sup.SupplierID
                    LEFT JOIN Categories cata ON pr.CategoryID = cata.CategoryID
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[LaySanPhamTheoId] (@ProductID INT)
                AS BEGIN
                    SELECT ProductID, ProductName, UnitPrice, QuantityPerUnit, CategoryID, SupplierID FROM Products WHERE ProductID = @ProductID
                END
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROC [dbo].[ThemSanPham] (@ProductName nvarchar(40), @UnitPrice money, @QuantityPerUnit nvarchar(20), @CategoryID int, @SupplierID int)
                AS BEGIN
                    INSERT INTO Products(ProductName, UnitPrice, QuantityPerUnit, CategoryID, SupplierID)
                    VALUES(@ProductName, @UnitPrice, @QuantityPerUnit, @CategoryID, @SupplierID)
                END
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROC [dbo].[SuaSanPham] (@ProductID int, @ProductName nvarchar(40), @UnitPrice money, @QuantityPerUnit nvarchar(20), @CategoryID int, @SupplierID int)
                AS BEGIN
                    UPDATE Products SET ProductName = @ProductName, UnitPrice = @UnitPrice, QuantityPerUnit = @QuantityPerUnit, CategoryID = @CategoryID, SupplierID = @SupplierID
                    WHERE ProductID = @ProductID
                END
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROC [dbo].[XoaSanPham] (@ProductID int)
                AS BEGIN
                    DELETE FROM Products WHERE ProductID = @ProductID
                END
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[DanhSachKhachHang]
                AS
                SELECT CustomerID, ContactName, Address, City, Country, Phone,Username,Password
                FROM Customers
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROC [dbo].[ThemKhachHang] 
                (@CustomerID nchar(5), @CompanyName nvarchar(40), @ContactName nvarchar(40), @Address nvarchar(60), @City nvarchar(15), @Country nvarchar(15), @Phone nvarchar(24), @Username nvarchar(50),@Password nvarchar(50))      
                AS BEGIN
                    INSERT INTO Customers(CustomerID,CompanyName,ContactName, Address, City, Country, Phone,Username,Password)
                    VALUES (@CustomerID,@CompanyName,@ContactName, @Address, @City, @Country, @Phone,@Username,@Password)
                END
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROC [dbo].[SuaKhachHang]
                (@CustomerID nchar(5),@CompanyName nvarchar(40),@ContactName nvarchar(40), @Address nvarchar(60), @City nvarchar(15), @Country nvarchar(15), @Phone nvarchar(24))
                AS BEGIN
                    UPDATE Customers
                    SET CompanyName = @CompanyName,ContactName = @ContactName, Address = @Address, City = @City, Country = @Country, Phone = @Phone
                    WHERE CustomerId = @CustomerID
                END
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROC [dbo].[XoaKhachHang] (@CustomerID nvarchar(450))
                AS BEGIN
                    DELETE FROM Customer WHERE CustomerId = @CustomerID
                END
            ");
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROC [dbo].[LayKhachHangTheoId]
                (@CustomerID nchar(5))
                AS BEGIN
                    SELECT CustomerID, ContactName, Address, City, Country, Phone,UserName,Password
                    FROM Customers
                    WHERE CustomerID = @CustomerID
                END
            ");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "CategoryReqs");

            //migrationBuilder.DropTable(
            //    name: "CustomerCustomerDemographic");

            //migrationBuilder.DropTable(
            //    name: "EmployeeTerritory");

            //migrationBuilder.DropTable(
            //    name: "OrderDetail");

            //migrationBuilder.DropTable(
            //    name: "SupplierReqs");

            //migrationBuilder.DropTable(
            //    name: "CustomerDemographic");

            //migrationBuilder.DropTable(
            //    name: "Territory");

            //migrationBuilder.DropTable(
            //    name: "Order");

            //migrationBuilder.DropTable(
            //    name: "Products");

            //migrationBuilder.DropTable(
            //    name: "Region");

            //migrationBuilder.DropTable(
            //    name: "Customer");

            //migrationBuilder.DropTable(
            //    name: "Employee");

            //migrationBuilder.DropTable(
            //    name: "Shipper");

            //migrationBuilder.DropTable(
            //    name: "Categories");

            //migrationBuilder.DropTable(
            //    name: "Suppliers");


            migrationBuilder.DropColumn(
        name: "Username",
        table: "Customer");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Employee");

            migrationBuilder.Sql(@"DROP VIEW IF EXISTS [dbo].[DanhSachSanPham]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[LaySanPhamTheoId]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[ThemSanPham]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[SuaSanPham]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[XoaSanPham]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[DanhSachKhachHang]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[ThemKhachHang]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[SuaKhachHang]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[XoaKhachHang]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[LayKhachHangTheoId]");
        }

    }
}
