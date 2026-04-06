using Microsoft.Data.SqlClient;
using QLBH.Common;
using System.Data;

namespace QLBH.DAL
{
    public class ProductDAL : DbConnect
    {
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }

        public List<ProductReq> getSanPham()
        {
            using var conn = CreateConnection();
            using var cmd = new SqlCommand(
                "SELECT p.ProductID, p.ProductName, p.UnitPrice, p.QuantityPerUnit, c.CategoryName, s.CompanyName FROM Products p LEFT JOIN Categories c ON p.CategoryID = c.CategoryID LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID", conn)
            {
                CommandType = CommandType.Text
            };
            using var da = new SqlDataAdapter(cmd);

            var dt = new DataTable();
            da.Fill(dt);

            var list = new List<ProductReq>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new ProductReq
                {
                    Id = Convert.ToInt32(row["ProductID"]),
                    Name = row["ProductName"]?.ToString() ?? string.Empty,
                    Price = row["UnitPrice"] != DBNull.Value ? Convert.ToDecimal(row["UnitPrice"]) : 0,
                    Quantity = row["QuantityPerUnit"]?.ToString() ?? string.Empty,
                });
            }
            return list;
        }

        public DataTable getSanPhamDataTable()
        {
            using var conn = CreateConnection();
            using var cmd = new SqlCommand(
                "SELECT p.ProductID, p.ProductName, p.UnitPrice, p.QuantityPerUnit, c.CategoryName, s.CompanyName FROM Products p LEFT JOIN Categories c ON p.CategoryID = c.CategoryID LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID", conn)
            {
                CommandType = CommandType.Text
            };
            using var da = new SqlDataAdapter(cmd);

            var dtSanPham = new DataTable();
            da.Fill(dtSanPham);
            return dtSanPham;
        }

        public bool themSanPham(ProductReq sp)
        {
            using var conn = CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand(@"INSERT INTO Products (ProductName, UnitPrice, QuantityPerUnit, CategoryID, SupplierID) 
                                             VALUES (@ProductName, @UnitPrice, @QuantityPerUnit, @CategoryID, @SupplierID)", conn)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 40).Value = sp.Name;
            cmd.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = sp.Price;
            cmd.Parameters.Add("@QuantityPerUnit", SqlDbType.NVarChar, 20).Value = sp.Quantity;
            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = sp.CateId;
            cmd.Parameters.Add("@SupplierID", SqlDbType.Int).Value = sp.SupId;

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool suaSanPham(ProductReq sp)
        {
            using var conn = CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand(@"UPDATE Products SET ProductName = @ProductName, UnitPrice = @UnitPrice, QuantityPerUnit = @QuantityPerUnit, CategoryID = @CategoryID, SupplierID = @SupplierID WHERE ProductID = @ProductID", conn)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = sp.Id;
            cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 40).Value = sp.Name;
            cmd.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = sp.Price;
            cmd.Parameters.Add("@QuantityPerUnit", SqlDbType.NVarChar, 20).Value = sp.Quantity;
            cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = sp.CateId;
            cmd.Parameters.Add("@SupplierID", SqlDbType.Int).Value = sp.SupId;

            return cmd.ExecuteNonQuery() > 0;
        }

        public ProductReq? laySanPhamTheoId(int idSP)
        {
            using var conn = CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand("SELECT ProductID, ProductName, UnitPrice, QuantityPerUnit, CategoryID, SupplierID FROM Products WHERE ProductID = @ProductID", conn)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = idSP;

            using var dr = cmd.ExecuteReader();
            if (!dr.Read())
            {
                return null;
            }

            return new ProductReq
            {
                Id = Convert.ToInt32(dr["ProductID"]),
                Name = dr["ProductName"]?.ToString() ?? string.Empty,
                Price = Convert.ToDecimal(dr["UnitPrice"]),
                Quantity = dr["QuantityPerUnit"]?.ToString() ?? string.Empty,
                CateId = Convert.ToInt32(dr["CategoryID"]),
                SupId = Convert.ToInt32(dr["SupplierID"])
            };
        }

        public bool xoaSanPham(int idSP)
        {
            using var conn = CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand("DELETE FROM Products WHERE ProductID = @ProductID", conn)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = idSP;

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
