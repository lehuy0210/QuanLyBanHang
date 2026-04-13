using Microsoft.Data.SqlClient;
using QLBH.Common;
using QLBH.DAL.Models;
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
            string query = "SELECT * FROM DanhSachDonHang";

            SqlCommand cmd = new SqlCommand(query, _conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtDonHang= new DataTable();
            da.Fill(dtDonHang);
            return dtDonHang;
        }

        public List<OrderReq> layDonHangTheoId(int orderid)
        {
            List<OrderReq> lstOrderDetails = new List<OrderReq>();
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
                        OrderReq order = new OrderReq();
                    order.OrderID = Convert.ToInt32(dr["OrderID"]);
                    order.ContactName = dr["ContactName"].ToString();
                    order.ProductName = dr["ProductName"].ToString();
                    order.UnitPrice = Convert.ToDecimal(dr["UnitPrice"]);
                    order.Quantity = Convert.ToInt32(dr["Quantity"]);
                    order.TotalPrice = Convert.ToDecimal(dr["Total Price"]);
                    lstOrderDetails.Add(order);

                }

            }
            catch(Exception ex)
            {
               
            }
            finally
            {
                _conn.Close();
            }
            return lstOrderDetails;
        }
    }
}
