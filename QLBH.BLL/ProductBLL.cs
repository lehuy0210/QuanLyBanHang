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
    public class ProductBLL
    {
        ProductDAL dalSanPham = new ProductDAL();

        public DataTable getSanPham()
        {
            return dalSanPham.getSanPham();
        }

        public DataTable getSanPhamTimKiem(string tukhoa)
        {
            return dalSanPham.getSanPhamTimKiem(tukhoa);
        }
        public bool themSanPham(ProductDTO sp)
        {
            return dalSanPham.themSanPham(sp);
        }

        public bool suaSanPham(ProductDTO sp)
        {
            return dalSanPham.suaSanPham(sp);
        }

        public ProductDTO laySanPhamTheoId(int idSP)
        {
            return dalSanPham.laySanPhamTheoId(idSP);
        }

        public bool xoaSanPham(int idSP)
        {
            return dalSanPham.xoaSanPham(idSP);
        }
    }
}
