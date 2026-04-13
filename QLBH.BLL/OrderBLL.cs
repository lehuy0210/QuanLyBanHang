using QLBH.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH.BLL
{
    public class OrderBLL
    {
        OrderDAL dalDonHang = new OrderDAL();
        public DataTable getDonHang()
        {
            return dalDonHang.getDonHang();
        }
    }
}
