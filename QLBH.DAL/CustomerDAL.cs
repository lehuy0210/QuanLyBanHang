using Microsoft.Data.SqlClient;
using QLBH.Common;
using System.Data;

namespace QLBH.DAL
{
    public class CustomerDAL : DbConnect
    {
        public CustomerDAL(string connectionString) : base(connectionString)
        {
        }

        public List<CustomerReq> getKhachHang()
        {
            using var conn = CreateConnection();
            using var cmd = new SqlCommand(
                "SELECT CustomerID, ContactName, Address, City, Country, Phone FROM Customers", conn)
            {
                CommandType = CommandType.Text
            };
            using var da = new SqlDataAdapter(cmd);

            var dt = new DataTable();
            da.Fill(dt);

            var list = new List<CustomerReq>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new CustomerReq
                {
                    Id = row["CustomerID"]?.ToString() ?? string.Empty,
                    Name = row["ContactName"]?.ToString() ?? string.Empty,
                    Address = row["Address"]?.ToString() ?? string.Empty,
                    City = row["City"]?.ToString() ?? string.Empty,
                    Country = row["Country"]?.ToString() ?? string.Empty,
                    Phone = row["Phone"]?.ToString() ?? string.Empty,
                });
            }
            return list;
        }

        public CustomerReq? layKhachHangTheoId(string id)
        {
            using var conn = CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand(
                "SELECT CustomerID, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax FROM Customers WHERE CustomerID = @CustomerID", conn)
            {
                CommandType = CommandType.Text
            };
            cmd.Parameters.Add("@CustomerID", SqlDbType.NChar, 5).Value = id;

            using var dr = cmd.ExecuteReader();
            if (!dr.Read())
            {
                return null;
            }

            return new CustomerReq
            {
                Id = dr["CustomerID"]?.ToString()?.Trim() ?? string.Empty,
                Name = dr["ContactName"]?.ToString() ?? string.Empty,
                Address = dr["Address"]?.ToString() ?? string.Empty,
                City = dr["City"]?.ToString() ?? string.Empty,
                Country = dr["Country"]?.ToString() ?? string.Empty,
                Phone = dr["Phone"]?.ToString() ?? string.Empty,
            };
        }

        public bool themKhachHang(CustomerReq kh)
        {
            using var conn = CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand(
                @"INSERT INTO Customers (CustomerID, CompanyName, ContactName, Address, City, Country, Phone) 
                  VALUES (@CustomerID, @CompanyName, @ContactName, @Address, @City, @Country, @Phone)", conn)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.Add("@CustomerID", SqlDbType.NChar, 5).Value = kh.Id.ToUpper();
            cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 40).Value = kh.Name;
            cmd.Parameters.Add("@ContactName", SqlDbType.NVarChar, 30).Value = kh.Name;
            cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 60).Value = string.IsNullOrWhiteSpace(kh.Address) ? DBNull.Value : kh.Address;
            cmd.Parameters.Add("@City", SqlDbType.NVarChar, 15).Value = string.IsNullOrWhiteSpace(kh.City) ? DBNull.Value : kh.City;
            cmd.Parameters.Add("@Country", SqlDbType.NVarChar, 15).Value = string.IsNullOrWhiteSpace(kh.Country) ? DBNull.Value : kh.Country;
            cmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 24).Value = string.IsNullOrWhiteSpace(kh.Phone) ? DBNull.Value : kh.Phone;

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool suaKhachHang(CustomerReq kh)
        {
            using var conn = CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand(
                @"UPDATE Customers SET ContactName = @ContactName, Address = @Address, 
                  City = @City, Country = @Country, Phone = @Phone 
                  WHERE CustomerID = @CustomerID", conn)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.Add("@CustomerID", SqlDbType.NChar, 5).Value = kh.Id;
            cmd.Parameters.Add("@ContactName", SqlDbType.NVarChar, 30).Value = kh.Name;
            cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 60).Value = string.IsNullOrWhiteSpace(kh.Address) ? DBNull.Value : kh.Address;
            cmd.Parameters.Add("@City", SqlDbType.NVarChar, 15).Value = string.IsNullOrWhiteSpace(kh.City) ? DBNull.Value : kh.City;
            cmd.Parameters.Add("@Country", SqlDbType.NVarChar, 15).Value = string.IsNullOrWhiteSpace(kh.Country) ? DBNull.Value : kh.Country;
            cmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 24).Value = string.IsNullOrWhiteSpace(kh.Phone) ? DBNull.Value : kh.Phone;

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool xoaKhachHang(string id)
        {
            using var conn = CreateConnection();
            conn.Open();

            using var cmd = new SqlCommand("DELETE FROM Customers WHERE CustomerID = @CustomerID", conn)
            {
                CommandType = CommandType.Text
            };
            cmd.Parameters.Add("@CustomerID", SqlDbType.NChar, 5).Value = id;

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
