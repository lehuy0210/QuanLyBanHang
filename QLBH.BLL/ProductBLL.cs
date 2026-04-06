using QLBH.Common;
using QLBH.DAL;
using System.Data;

namespace QLBH.BLL
{
    public class ProductBLL
    {
        private readonly ProductDAL _dalSanPham;

        public ProductBLL(ProductDAL dalSanPham)
        {
            _dalSanPham = dalSanPham ?? throw new ArgumentNullException(nameof(dalSanPham));
        }

        public List<ProductReq> getSanPham()
        {
            return _dalSanPham.getSanPham();
        }

        public DataTable getSanPhamDataTable()
        {
            return _dalSanPham.getSanPhamDataTable();
        }

        public bool themSanPham(ProductReq sp)
        {
            ValidateProduct(sp);
            return _dalSanPham.themSanPham(sp);
        }

        public bool suaSanPham(ProductReq sp)
        {
            if (sp.Id <= 0)
                throw new ArgumentException("Mã sản phẩm không hợp lệ.");
            ValidateProduct(sp);
            return _dalSanPham.suaSanPham(sp);
        }

        public ProductReq? laySanPhamTheoId(int idSP)
        {
            if (idSP <= 0)
                throw new ArgumentException("Mã sản phẩm không hợp lệ.");
            return _dalSanPham.laySanPhamTheoId(idSP);
        }

        public bool xoaSanPham(int idSP)
        {
            if (idSP <= 0)
                throw new ArgumentException("Mã sản phẩm không hợp lệ.");
            return _dalSanPham.xoaSanPham(idSP);
        }

        private void ValidateProduct(ProductReq sp)
        {
            if (string.IsNullOrWhiteSpace(sp.Name))
                throw new ArgumentException("Tên sản phẩm không được để trống.");
            if (sp.Name.Length > 40)
                throw new ArgumentException("Tên sản phẩm tối đa 40 ký tự.");
            if (sp.Price < 0)
                throw new ArgumentException("Đơn giá phải >= 0.");
            if (sp.CateId <= 0)
                throw new ArgumentException("Vui lòng chọn loại sản phẩm.");
            if (sp.SupId <= 0)
                throw new ArgumentException("Vui lòng chọn nhà cung cấp.");
        }
    }
}
