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

        public bool themKhachHang(CustomerReq kh)
        {
            try
            {
                _conn.Open();

                string tenProc = "ThemKhachHang";
                SqlCommand cmd = new SqlCommand(tenProc, _conn);
                cmd.CommandType = CommandType.StoredProcedure;

                Random rnd = new Random();
                string idXuLy = "KH" + rnd.Next(100, 999).ToString();

                cmd.Parameters.Add("@CustomerID", SqlDbType.NChar, 5).Value = idXuLy;
                cmd.Parameters.Add("@ContactName", SqlDbType.NVarChar, 40).Value = kh.Name;
                cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 40).Value = "Cá nhân";
                cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 60).Value = kh.Address;
                cmd.Parameters.Add("@City", SqlDbType.NVarChar, 15).Value = kh.City;
                cmd.Parameters.Add("@Country", SqlDbType.NVarChar, 15).Value = kh.Country;
                cmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 24).Value = kh.Phone;
                cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = kh.Username;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = kh.Password;

                if (cmd.ExecuteNonQuery() > 0)
                {
                    return  true;
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                _conn.Close();
            }
            return false;
        }

        public CustomerReq layKHTheoID(string idKH)
        {
            CustomerReq kh = null;
            try
            {
                _conn.Open();

                string tenProc = "LayKhachHangTheoId";
                SqlCommand cmd = new SqlCommand(tenProc, _conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CustomerID", SqlDbType.Char, 5).Value = idKH;

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    kh = new CustomerReq();
                    kh.Id = dr["CustomerID"].ToString();
                    kh.Name = dr["ContactName"].ToString();
                    kh.Address = dr["Address"].ToString();
                    kh.City = dr["City"].ToString();
                    kh.Country = dr["Country"].ToString();
                    kh.Phone = dr["Phone"].ToString();
                    kh.Username = dr["Username"].ToString();
                    kh.Password = dr["Password"].ToString();
                }
                dr.Close();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                _conn.Close();
            }
            return kh;  
        }
        public bool xoaKhachHang(string idKH)
        {
            try
            {
                _conn.Open();

                string tenProc = "XoaKhachHang";
                SqlCommand cmd = new SqlCommand(tenProc, _conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CustomerID", SqlDbType.Char, 5).Value = idKH;


                if (cmd.ExecuteNonQuery() > 0)
                {
                    return true;
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                _conn.Close();
            }
            return false;
        }

        public bool suaKhachHang(CustomerReq kh)
        {
            try
            {
                _conn.Open();

                string tenProc = "SuaKhachHang";
                SqlCommand cmd = new SqlCommand(tenProc, _conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@CustomerID", SqlDbType.NChar, 5).Value = kh.Id;
                cmd.Parameters.Add("@ContactName", SqlDbType.NVarChar, 40).Value = kh.Name;
                cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 40).Value = "Cá nhân";
                cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 60).Value = kh.Address;
                cmd.Parameters.Add("@City", SqlDbType.NVarChar, 15).Value = kh.City;
                cmd.Parameters.Add("@Country", SqlDbType.NVarChar, 15).Value = kh.Country;
                cmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 24).Value = kh.Phone;
                cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = kh.Username;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 50).Value = kh.Password;

                if(cmd.ExecuteNonQuery() > 0)
                {
                    return true;
                }

            }
            catch(Exception ex)
            {

            }
            finally
            {
                _conn.Close();
            }
            return false;
        }



    }
}
