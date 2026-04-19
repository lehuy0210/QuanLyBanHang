using QLBH.DAL;
using QLBH.DTO;
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

        public List<OrderDTO> layDonHangTheoId(int orderid)
        {
            return dalDonHang.layDonHangTheoId(orderid);
        }

        public bool XoaDonHang(int orderid)
        {
            return dalDonHang.XoaDonHang(orderid);
        }
    }
}
