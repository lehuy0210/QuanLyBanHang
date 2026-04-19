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
    public class CustomerBLL
    {
        CustomerDAL dalKhachHang = new CustomerDAL();

        public DataTable getKhachHang()
        {
            return dalKhachHang.getKhachHang();
        }

        public bool themKhachHang(CustomerDTO kh)
        {
            return dalKhachHang.themKhachHang(kh);
        }

        public CustomerDTO layKHTheoID(string idKH)
        {
            return dalKhachHang.layKHTheoID(idKH);
        }

        public bool xoaKhachHang(string idKH)
        {
            return dalKhachHang.xoaKhachHang(idKH);
        }

        public bool suaKhachHang(CustomerDTO kh)
        {
            return dalKhachHang.suaKhachHang(kh);
        }
    }
}
