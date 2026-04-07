using QLBH.Common;
using QLBH.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBH.BLL
{
    public class CustomerBLL
    {
        CustomerDAL dalKhachHang = new CustomerDAL();

        public DataTable getKhachHang()
        {
            return dalKhachHang.getKhachHang();
        }

        public bool themKhachHang(CustomerReq kh)
        {
            return dalKhachHang.themKhachHang(kh);
        }

        public CustomerReq layKHTheoID(string idKH)
        {
            return dalKhachHang.layKHTheoID(idKH);
        }

        public bool xoaKhachHang(string idKH)
        {
            return dalKhachHang.xoaKhachHang(idKH);
        }

        public bool suaKhachHang(CustomerReq kh)
        {
            return dalKhachHang.suaKhachHang(kh);
        }
    }
}
