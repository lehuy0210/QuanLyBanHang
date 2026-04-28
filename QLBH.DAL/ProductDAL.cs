using Microsoft.Data.SqlClient;
using QLBH.DTO;
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
        public DataTable getSanPham()
        {
            string query = "SELECT * FROM DanhSachSanPham";

            SqlCommand cmd = new SqlCommand(query, _conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtSanPham = new DataTable();
            da.Fill(dtSanPham);
            return dtSanPham;
        }

        public DataTable getSanPhamTimKiem(string tukhoa)
        {
            string tenProc = "TimKiemSanPham";
            SqlCommand cmd = new SqlCommand(tenProc, _conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 40).Value = tukhoa;
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dtTimKiem = new DataTable();
            da.Fill(dtTimKiem);
            return dtTimKiem;
        }

        public bool themSanPham(ProductDTO sp)
        {
            try
            {
                _conn.Open();

                string tenProc = "ThemSanPham";
                SqlCommand cmd = new SqlCommand(tenProc, _conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 40).Value = sp.Name;
                cmd.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = sp.Price;
                cmd.Parameters.Add("@QuantityPerUnit", SqlDbType.NVarChar, 20).Value = sp.Quantity;
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = sp.CateId;
                cmd.Parameters.Add("@SupplierID", SqlDbType.Int).Value = sp.SupId;
                cmd.Parameters.Add("@UnitsInStock", SqlDbType.SmallInt).Value = sp.UnitsInStock;

                if(cmd.ExecuteNonQuery() > 0)
                {
                    return true;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _conn.Close();
            }

            return false;
        }

        public bool suaSanPham(ProductDTO sp)
        {
            try
            {
                _conn.Open();

                string tenProc = "SuaSanPham";
                SqlCommand cmd = new SqlCommand(tenProc, _conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = sp.Id;
                cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 40).Value = sp.Name;
                cmd.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = sp.Price;
                cmd.Parameters.Add("@QuantityPerUnit", SqlDbType.NVarChar, 20).Value = sp.Quantity;
                cmd.Parameters.Add("@CategoryID", SqlDbType.Int).Value = sp.CateId;
                cmd.Parameters.Add("@SupplierID", SqlDbType.Int).Value = sp.SupId;
                cmd.Parameters.Add("@UnitsInStock", SqlDbType.SmallInt).Value = sp.UnitsInStock;

                if (cmd.ExecuteNonQuery() > 0)
                {
                    return true;
                }

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _conn.Close();
            }
            return false;
        }

        public ProductDTO laySanPhamTheoId(int idSP)
        {
            ProductDTO sp = null;
            try
            {
                _conn.Open();

                string tenProc = "LaySanPhamTheoId";
                SqlCommand cmd = new SqlCommand(tenProc, _conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = idSP;

                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.Read())
                {
                    sp = new ProductDTO();
                    sp.Id = Convert.ToInt32(dr["ProductID"]);
                    sp.Name = dr["ProductName"].ToString();
                    sp.Price = Convert.ToDecimal(dr["UnitPrice"]);
                    sp.Quantity = dr["QuantityPerUnit"].ToString();
                    sp.CateId = Convert.ToInt32(dr["CategoryID"]);
                    sp.SupId = Convert.ToInt32(dr["SupplierID"]);
                    sp.UnitsInStock = Convert.ToInt32(dr["UnitsInStock"]);
                }
                dr.Close();

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _conn.Close();
            }
            return sp;
        }

        public bool xoaSanPham(int idSP)
        {
            try
            {
                _conn.Open();

                string tenProc = "XoaSanPham";
                SqlCommand cmd = new SqlCommand(tenProc, _conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = idSP;

                if (cmd.ExecuteNonQuery() > 0)
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _conn.Close();
            }

            return false;
            
        }
    }
}
