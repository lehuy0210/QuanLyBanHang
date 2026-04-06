using Microsoft.Data.SqlClient;
using QLBH.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH.DAL
{
    public class ProductDAL : DbConnect
    {
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }

        public DataTable getSanPham()
        {
            using var conn = CreateConnection();
            using var cmd = new SqlCommand("XemDanhSachSanPham", conn)
            {
                CommandType = CommandType.StoredProcedure
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

            using var cmd = new SqlCommand("ThemSanPham", conn)
            {
                CommandType = CommandType.StoredProcedure
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

            using var cmd = new SqlCommand("SuaSanPham", conn)
            {
                CommandType = CommandType.StoredProcedure
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

            using var cmd = new SqlCommand("LaySanPhamTheoId", conn)
            {
                CommandType = CommandType.StoredProcedure
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

            using var cmd = new SqlCommand("XoaSanPham", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = idSP;

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
