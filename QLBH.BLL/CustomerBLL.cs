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
    }
}
