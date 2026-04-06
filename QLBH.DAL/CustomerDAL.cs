using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH.DAL
{
    public class CustomerDAL : DbConnect
    {
        public DataTable getKhachHang()
        {
            string query = "SELECT * FROM DanhSachKhachHang";
            SqlCommand cmd = new SqlCommand(query, _conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtKhachHang = new DataTable();
            da.Fill(dtKhachHang);
            return dtKhachHang;
        }


    }
}
