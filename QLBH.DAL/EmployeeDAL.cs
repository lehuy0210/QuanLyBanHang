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
    public class EmployeeDAL : DbConnect
    {
        public DataTable getKhachHang()
        {
            string query = "SELECT * FROM DanhSachNhanVien";
            SqlCommand cmd = new SqlCommand(query, _conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtNhanVien = new DataTable();
            da.Fill(dtNhanVien);
            return dtNhanVien;
        }

        public bool themNhanVien(EmployeeDTO nv)
        {
            try
            {
                _conn.Open();

                string tenProc = "ThemNhanVien";
                SqlCommand cmd = new SqlCommand(tenProc, _conn);
                cmd.CommandType = CommandType.StoredProcedure;

                Random rnd = new Random();

                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 20).Value = nv.LastName;
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 10).Value = nv.FirstName;
                cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 60).Value = nv.Address;
                cmd.Parameters.Add("@City", SqlDbType.NVarChar, 15).Value = nv.City;
                cmd.Parameters.Add("@Country", SqlDbType.NVarChar, 15).Value = nv.Country;
                cmd.Parameters.Add("@HomePhone", SqlDbType.NVarChar, 24).Value = nv.Phone;
                nv.Username = (nv.LastName + nv.FirstName).Replace(" ", "").ToLower();
                cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = nv.Username;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 255).Value = nv.Password;

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

        public EmployeeDTO layNVTheoID(int idNV)
        {
            EmployeeDTO nv = null;
            try
            {
                _conn.Open();

                string tenProc = "LayNhanVienTheoId";
                SqlCommand cmd = new SqlCommand(tenProc, _conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@EmployeeID",SqlDbType.Int).Value = idNV;

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    nv = new EmployeeDTO();
                    nv.Id = dr["EmployeeID"].ToString();
                    nv.LastName = dr["LastName"].ToString();
                    nv.FirstName= dr["FirstName"].ToString();
                    nv.Address = dr["Address"].ToString();
                    nv.City = dr["City"].ToString();
                    nv.Country = dr["Country"].ToString();
                    nv.Phone = dr["HomePhone"].ToString();
                    nv.Username = dr["Username"].ToString();
                    nv.Password = dr["Password"].ToString();
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _conn.Close();
            }
            return nv;
        }

        public bool xoaNhanVien(int idNV)
        {
            try
            {
                _conn.Open();

                string tenProc = "XoaNhanVien";
                SqlCommand cmd = new SqlCommand(tenProc, _conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = idNV;


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

        public bool suaNhanVien(EmployeeDTO nv)
        {
            try
            {
                _conn.Open();

                string tenProc = "SuaNhanVien";
                SqlCommand cmd = new SqlCommand(tenProc, _conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@EmployeeID ", SqlDbType.Int).Value = nv.Id;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 20).Value = nv.LastName;
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 10).Value = nv.FirstName;
                cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 60).Value = nv.Address;
                cmd.Parameters.Add("@City", SqlDbType.NVarChar, 15).Value = nv.City;
                cmd.Parameters.Add("@Country", SqlDbType.NVarChar, 15).Value = nv.Country;
                cmd.Parameters.Add("@HomePhone", SqlDbType.NVarChar, 24).Value = nv.Phone;
                cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = nv.Username;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 255).Value = nv.Password;

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
