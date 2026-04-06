using Microsoft.Data.SqlClient;
using QLBH.Common;
using System.Data;

namespace QLBH.DAL
{
    public class OrderDAL : DbConnect
    {
        public OrderDAL(string connectionString) : base(connectionString)
        {
        }

        public List<OrderReq> getOrders()
        {
            using var conn = CreateConnection();
            using var cmd = new SqlCommand(
                @"SELECT o.OrderID, o.CustomerID, c.ContactName AS CustomerName,
                         o.EmployeeID, (e.FirstName + ' ' + e.LastName) AS EmployeeName,
                         o.OrderDate, o.RequiredDate, o.ShippedDate, o.Freight,
                         o.ShipName, o.ShipAddress, o.ShipCity, o.ShipCountry
                  FROM Orders o
                  LEFT JOIN Customers c ON o.CustomerID = c.CustomerID
                  LEFT JOIN Employees e ON o.EmployeeID = e.EmployeeID
                  ORDER BY o.OrderDate DESC", conn)
            {
                CommandType = CommandType.Text
            };
            using var da = new SqlDataAdapter(cmd);

            var dt = new DataTable();
            da.Fill(dt);

            var list = new List<OrderReq>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new OrderReq
                {
                    Id = Convert.ToInt32(row["OrderID"]),
                    CustomerId = row["CustomerID"]?.ToString()?.Trim() ?? string.Empty,
                    CustomerName = row["CustomerName"]?.ToString() ?? string.Empty,
                    EmployeeId = row["EmployeeID"] != DBNull.Value ? Convert.ToInt32(row["EmployeeID"]) : null,
                    EmployeeName = row["EmployeeName"]?.ToString() ?? string.Empty,
                    OrderDate = row["OrderDate"] != DBNull.Value ? Convert.ToDateTime(row["OrderDate"]) : null,
                    RequiredDate = row["RequiredDate"] != DBNull.Value ? Convert.ToDateTime(row["RequiredDate"]) : null,
                    ShippedDate = row["ShippedDate"] != DBNull.Value ? Convert.ToDateTime(row["ShippedDate"]) : null,
                    Freight = row["Freight"] != DBNull.Value ? Convert.ToDecimal(row["Freight"]) : 0,
                    ShipName = row["ShipName"]?.ToString(),
                    ShipAddress = row["ShipAddress"]?.ToString(),
                    ShipCity = row["ShipCity"]?.ToString(),
                    ShipCountry = row["ShipCountry"]?.ToString(),
                });
            }
            return list;
        }

        public OrderReq? getOrderById(int id)
        {
            using var conn = CreateConnection();
            conn.Open();

            // Get order header
            using var cmd = new SqlCommand(
                @"SELECT o.OrderID, o.CustomerID, c.ContactName AS CustomerName,
                         o.EmployeeID, (e.FirstName + ' ' + e.LastName) AS EmployeeName,
                         o.OrderDate, o.RequiredDate, o.ShippedDate, o.ShipVia, o.Freight,
                         o.ShipName, o.ShipAddress, o.ShipCity, o.ShipRegion, o.ShipPostalCode, o.ShipCountry
                  FROM Orders o
                  LEFT JOIN Customers c ON o.CustomerID = c.CustomerID
                  LEFT JOIN Employees e ON o.EmployeeID = e.EmployeeID
                  WHERE o.OrderID = @OrderID", conn)
            {
                CommandType = CommandType.Text
            };
            cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = id;

            OrderReq? order = null;
            using (var dr = cmd.ExecuteReader())
            {
                if (!dr.Read()) return null;

                order = new OrderReq
                {
                    Id = Convert.ToInt32(dr["OrderID"]),
                    CustomerId = dr["CustomerID"]?.ToString()?.Trim() ?? string.Empty,
                    CustomerName = dr["CustomerName"]?.ToString() ?? string.Empty,
                    EmployeeId = dr["EmployeeID"] != DBNull.Value ? Convert.ToInt32(dr["EmployeeID"]) : null,
                    EmployeeName = dr["EmployeeName"]?.ToString() ?? string.Empty,
                    OrderDate = dr["OrderDate"] != DBNull.Value ? Convert.ToDateTime(dr["OrderDate"]) : null,
                    RequiredDate = dr["RequiredDate"] != DBNull.Value ? Convert.ToDateTime(dr["RequiredDate"]) : null,
                    ShippedDate = dr["ShippedDate"] != DBNull.Value ? Convert.ToDateTime(dr["ShippedDate"]) : null,
                    ShipVia = dr["ShipVia"] != DBNull.Value ? Convert.ToInt32(dr["ShipVia"]) : null,
                    Freight = dr["Freight"] != DBNull.Value ? Convert.ToDecimal(dr["Freight"]) : 0,
                    ShipName = dr["ShipName"]?.ToString(),
                    ShipAddress = dr["ShipAddress"]?.ToString(),
                    ShipCity = dr["ShipCity"]?.ToString(),
                    ShipRegion = dr["ShipRegion"]?.ToString(),
                    ShipPostalCode = dr["ShipPostalCode"]?.ToString(),
                    ShipCountry = dr["ShipCountry"]?.ToString(),
                };
            }

            // Get order details
            using var cmdDetails = new SqlCommand(
                @"SELECT od.OrderID, od.ProductID, p.ProductName, od.UnitPrice, od.Quantity, od.Discount
                  FROM [Order Details] od
                  INNER JOIN Products p ON od.ProductID = p.ProductID
                  WHERE od.OrderID = @OrderID", conn)
            {
                CommandType = CommandType.Text
            };
            cmdDetails.Parameters.Add("@OrderID", SqlDbType.Int).Value = id;

            using var drDetails = cmdDetails.ExecuteReader();
            while (drDetails.Read())
            {
                order.OrderDetails.Add(new OrderDetailReq
                {
                    OrderId = Convert.ToInt32(drDetails["OrderID"]),
                    ProductId = Convert.ToInt32(drDetails["ProductID"]),
                    ProductName = drDetails["ProductName"]?.ToString() ?? string.Empty,
                    UnitPrice = Convert.ToDecimal(drDetails["UnitPrice"]),
                    Quantity = Convert.ToInt16(drDetails["Quantity"]),
                    Discount = Convert.ToSingle(drDetails["Discount"]),
                });
            }

            return order;
        }

        public int themOrder(OrderReq order)
        {
            using var conn = CreateConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                // Insert Order header
                using var cmd = new SqlCommand(
                    @"INSERT INTO Orders (CustomerID, EmployeeID, OrderDate, RequiredDate, ShipVia, Freight, 
                                          ShipName, ShipAddress, ShipCity, ShipRegion, ShipPostalCode, ShipCountry)
                      VALUES (@CustomerID, @EmployeeID, @OrderDate, @RequiredDate, @ShipVia, @Freight,
                              @ShipName, @ShipAddress, @ShipCity, @ShipRegion, @ShipPostalCode, @ShipCountry);
                      SELECT CAST(SCOPE_IDENTITY() AS INT);", conn, transaction)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.Add("@CustomerID", SqlDbType.NChar, 5).Value = order.CustomerId;
                cmd.Parameters.Add("@EmployeeID", SqlDbType.Int).Value = (object?)order.EmployeeId ?? DBNull.Value;
                cmd.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = order.OrderDate ?? DateTime.Now;
                cmd.Parameters.Add("@RequiredDate", SqlDbType.DateTime).Value = (object?)order.RequiredDate ?? DBNull.Value;
                cmd.Parameters.Add("@ShipVia", SqlDbType.Int).Value = (object?)order.ShipVia ?? DBNull.Value;
                cmd.Parameters.Add("@Freight", SqlDbType.Money).Value = order.Freight ?? 0m;
                cmd.Parameters.Add("@ShipName", SqlDbType.NVarChar, 40).Value = (object?)order.ShipName ?? DBNull.Value;
                cmd.Parameters.Add("@ShipAddress", SqlDbType.NVarChar, 60).Value = (object?)order.ShipAddress ?? DBNull.Value;
                cmd.Parameters.Add("@ShipCity", SqlDbType.NVarChar, 15).Value = (object?)order.ShipCity ?? DBNull.Value;
                cmd.Parameters.Add("@ShipRegion", SqlDbType.NVarChar, 15).Value = (object?)order.ShipRegion ?? DBNull.Value;
                cmd.Parameters.Add("@ShipPostalCode", SqlDbType.NVarChar, 10).Value = (object?)order.ShipPostalCode ?? DBNull.Value;
                cmd.Parameters.Add("@ShipCountry", SqlDbType.NVarChar, 15).Value = (object?)order.ShipCountry ?? DBNull.Value;

                var orderId = (int)cmd.ExecuteScalar()!;

                // Insert Order Details
                foreach (var detail in order.OrderDetails)
                {
                    using var cmdDetail = new SqlCommand(
                        @"INSERT INTO [Order Details] (OrderID, ProductID, UnitPrice, Quantity, Discount)
                          VALUES (@OrderID, @ProductID, @UnitPrice, @Quantity, @Discount)", conn, transaction)
                    {
                        CommandType = CommandType.Text
                    };
                    cmdDetail.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderId;
                    cmdDetail.Parameters.Add("@ProductID", SqlDbType.Int).Value = detail.ProductId;
                    cmdDetail.Parameters.Add("@UnitPrice", SqlDbType.Money).Value = detail.UnitPrice;
                    cmdDetail.Parameters.Add("@Quantity", SqlDbType.SmallInt).Value = detail.Quantity;
                    cmdDetail.Parameters.Add("@Discount", SqlDbType.Real).Value = detail.Discount;
                    cmdDetail.ExecuteNonQuery();
                }

                transaction.Commit();
                return orderId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public bool xoaOrder(int orderId)
        {
            using var conn = CreateConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                // Delete details first (FK constraint)
                using var cmdDetails = new SqlCommand(
                    "DELETE FROM [Order Details] WHERE OrderID = @OrderID", conn, transaction);
                cmdDetails.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderId;
                cmdDetails.ExecuteNonQuery();

                // Then delete header
                using var cmd = new SqlCommand(
                    "DELETE FROM Orders WHERE OrderID = @OrderID", conn, transaction);
                cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderId;
                var result = cmd.ExecuteNonQuery() > 0;

                transaction.Commit();
                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
