using Microsoft.Data.SqlClient;
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
    }
}
