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
            if(sp.Name == null)
            {
                throw new Exception("Tên sản phẩm không được để trống");
            }

            if (sp.UnitsInStock < 0)
            {
                throw new Exception("Số lượng trong kho không được âm");
            }

            if (sp.Quantity == null)
            {
                throw new Exception("Khối lượng không được để trống");
            }

            return dalSanPham.themSanPham(sp);
        }

        public bool suaSanPham(ProductDTO sp)
        {
            if (sp.Name == null)
            {
                throw new Exception("Tên sản phẩm không được để trống");
            }

            if(sp.UnitsInStock < 0)
            {
                throw new Exception("Số lượng trong kho không được âm");
            }

            if(sp.Quantity == null)
            {
                throw new Exception("Khối lượng không được để trống");
            }

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
