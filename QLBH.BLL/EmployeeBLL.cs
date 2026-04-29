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
            if(nv.LastName == "")
            {
                throw new Exception("Họ không được để trống");
            }

            if (nv.FirstName == "")
            {
                throw new Exception("Tên không được để trống");
            }

            if (nv.Phone.Length < 10 || nv.Phone.Length > 11)
            {
                throw new Exception("Số điện thoại không hợp lệ (phải từ 10 đến 11 số)");
            }

            for (int i = 0; i < nv.Phone.Length; i++)
            {
                if (nv.Phone[i] < '0' || nv.Phone[i] > '9')
                {
                    throw new Exception("Số điện thoại sai cú pháp");
                }
            }

            if (nv.Address == "")
            {
                throw new Exception("Địa chỉ không được để trống");
            }


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
            if (nv.LastName == null)
            {
                throw new Exception("Họ không được để trống");
            }

            if (nv.FirstName == null)
            {
                throw new Exception("Tên không được để trống");
            }

            if (nv.Phone.Length < 10 || nv.Phone.Length > 11)
            {
                throw new Exception("Số điện thoại không hợp lệ (phải từ 10 đến 11 số)");
            }

            for (int i = 0; i < nv.Phone.Length; i++)
            {
                if (nv.Phone[i] < '0' && nv.Phone[i] > '9')
                {
                    throw new Exception("Số điện thoại sai cú pháp");
                }
            }

            if (nv.Address == null)
            {
                throw new Exception("Địa chỉ không được để trống");
            }


            return dalNV.suaNhanVien(nv);
        }
    }
}
