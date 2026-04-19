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
    public class EmployeeBLL
    {
        EmployeeDAL dalNV = new EmployeeDAL();

        public DataTable getNhanVien()
        {
            return dalNV.getKhachHang();
        }

        public bool themKhachHang(EmployeeDTO nv)
        {
            return dalNV.themKhachHang(nv);
        }

        public EmployeeDTO layKHTheoID(int idNV)
        {
            return dalNV.layNVTheoID(idNV);
        }

        public bool xoaKhachHang(int idNV)
        {
            return dalNV.xoaKhachHang(idNV);
        }

        public bool suaKhachHang(EmployeeDTO nv)
        {
            return dalNV.suaNhanVien(nv);
        }
    }
}
