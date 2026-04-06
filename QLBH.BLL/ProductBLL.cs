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
    public class ProductBLL
    {
        private readonly ProductDAL _dalSanPham;

        public ProductBLL(ProductDAL dalSanPham)
        {
            _dalSanPham = dalSanPham;
        }

        public DataTable getSanPham()
        {
            return _dalSanPham.getSanPham();
        }

        public bool themSanPham(ProductReq sp)
        {
            return _dalSanPham.themSanPham(sp);
        }

        public bool suaSanPham(ProductReq sp)
        {
            return _dalSanPham.suaSanPham(sp);
        }

        public ProductReq? laySanPhamTheoId(int idSP)
        {
            return _dalSanPham.laySanPhamTheoId(idSP);
        }

        public bool xoaSanPham(int idSP)
        {
            return _dalSanPham.xoaSanPham(idSP);
        }
    }
}
