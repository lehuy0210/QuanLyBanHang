using Microsoft.Data.SqlClient;
using QLBH.DAL.Models;
using QLBH.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH.DAL
{
    public class OrderDAL : DbConnect
    {
        public DataTable getDonHang()
        {
            string query = "SELECT * FROM DanhSachDonHang ORDER BY OrderID DESC";

            SqlCommand cmd = new SqlCommand(query, _conn);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtDonHang = new DataTable();
            da.Fill(dtDonHang);
            return dtDonHang;
        }

        public List<OrderDTO> layDonHangTheoId(int orderid)
        {
            List<OrderDTO> lstOrderDetails = new List<OrderDTO>();
            try
            {
                _conn.Open();

                string tenProc = "LayDonHangTheoId";
                SqlCommand cmd = new SqlCommand(tenProc, _conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@OrderId", SqlDbType.Int).Value = orderid;

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    OrderDTO order = new OrderDTO();
                    order.OrderID = Convert.ToInt32(dr["OrderID"]);
                    order.ContactName = dr["ContactName"].ToString();
                    order.ProductName = dr["ProductName"].ToString();
                    order.UnitPrice = Convert.ToDecimal(dr["UnitPrice"]);
                    order.Quantity = Convert.ToInt32(dr["Quantity"]);
                    order.TotalPrice = Convert.ToDecimal(dr["Total Price"]);
                    lstOrderDetails.Add(order);

                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                _conn.Close();
            }
            return lstOrderDetails;
        }

        public bool XoaDonHang(int orderid)
        {
            try
            {
                _conn.Open();

                string tenProc = "sp_DeleteOrder_RestoreStock";
                SqlCommand cmd = new SqlCommand(tenProc, _conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderid;

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
