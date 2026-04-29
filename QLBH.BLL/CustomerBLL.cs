using QLBH.DAL;
using QLBH.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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
            if (kh.Name == "")
            {
                throw new Exception("Họ và tên không được để trống");
            }

            if (kh.Username == "")
            {
                throw new Exception("Tên đăng nhập không được để trống");
            }

            if (kh.Password == "")
            {
                throw new Exception("Mật khẩu không được đễ trống");
            }

            if (kh.Phone == "")
            {
                throw new Exception("Số điện thoại không được để trống");
            }

            if (kh.Phone.Length < 10 || kh.Phone.Length > 11)
            {
                throw new Exception("Số điện thoại không hợp lệ (phải từ 10 đến 11 số)");
            }

            for (int i = 0; i < kh.Phone.Length; i++)
            {
                if (kh.Phone[i] < '0' && kh.Phone[i] > '9')
                {
                    throw new Exception("Số điện thoại sai cú pháp");
                }
            }

            if(kh.Address == "")
            {
                throw new Exception("Địa chỉ không được để trống");
            }


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
            if (kh.Name == null)
            {
                throw new Exception("Họ và tên không được để trống");
            }

            if (kh.Password == null)
            {
                throw new Exception("Mật khẩu không được đễ trống");
            }

            if (kh.Phone == null)
            {
                throw new Exception("Số điện thoại không được để trống");
            }

            if (kh.Phone.Length < 10 || kh.Phone.Length > 11)
            {
                throw new Exception("Số điện thoại không hợp lệ (phải từ 10 đến 11 số)");
            }

            for (int i = 0; i < kh.Phone.Length; i++)
            {
                if (kh.Phone[i] < '0' || kh.Phone[i] > '9')
                {
                    throw new Exception("Số điện thoại sai cú pháp");
                }
            }

            if (kh.Address == null)
            {
                throw new Exception("Địa chỉ không được để trống");
            }


            return dalKhachHang.suaKhachHang(kh);
        }
    }
}
