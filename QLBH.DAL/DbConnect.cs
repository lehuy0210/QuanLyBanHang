using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH.DAL
{
    public class DbConnect
    {
        protected SqlConnection _conn = new SqlConnection("Server=LAPTOP-5RU50CLF\\HUY;database=Northwind;user id=sa;password=123456;TrustServerCertificate=True;");
    }
}
