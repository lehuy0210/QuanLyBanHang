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
    public class EmployeeBLL
    {
        EmployeeDAL dalNV = new EmployeeDAL();

        public DataTable getNhanVien()
        {
            return dalNV.getKhachHang();
        }

        public bool themKhachHang(EmployeeReq nv)
        {
            return dalNV.themKhachHang(nv);
        }

        public EmployeeReq layKHTheoID(int idNV)
        {
            return dalNV.layNVTheoID(idNV);
        }

        public bool xoaKhachHang(int idNV)
        {
            return dalNV.xoaKhachHang(idNV);
        }

        public bool suaKhachHang(EmployeeReq nv)
        {
            return dalNV.suaNhanVien(nv);
        }
    }
}
