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

        public bool themNhanVien(EmployeeDTO nv)
        {
            return dalNV.themNhanVien(nv);
        }

        public EmployeeDTO layNVTheoID(int idNV)
        {
            return dalNV.layNVTheoID(idNV);
        }

        public bool xoaNhanVien(int idNV)
        {
            return dalNV.xoaNhanVien(idNV);
        }

        public bool suaNhanVien(EmployeeDTO nv)
        {
            return dalNV.suaNhanVien(nv);
        }
    }
}
