using QLBH.Common;
using QLBH.DAL;

namespace QLBH.BLL
{
    public class CustomerBLL
    {
        private readonly CustomerDAL _dalKhachHang;

        public CustomerBLL(CustomerDAL dalKhachHang)
        {
            _dalKhachHang = dalKhachHang ?? throw new ArgumentNullException(nameof(dalKhachHang));
        }

        public List<CustomerReq> getKhachHang()
        {
            return _dalKhachHang.getKhachHang();
        }

        public CustomerReq? layKhachHangTheoId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Mã khách hàng không hợp lệ.");
            return _dalKhachHang.layKhachHangTheoId(id);
        }

        public bool themKhachHang(CustomerReq kh)
        {
            ValidateCustomer(kh);
            if (string.IsNullOrWhiteSpace(kh.Id) || kh.Id.Length > 5)
                throw new ArgumentException("Mã khách hàng phải từ 1-5 ký tự.");
            return _dalKhachHang.themKhachHang(kh);
        }

        public bool suaKhachHang(CustomerReq kh)
        {
            if (string.IsNullOrWhiteSpace(kh.Id))
                throw new ArgumentException("Mã khách hàng không hợp lệ.");
            ValidateCustomer(kh);
            return _dalKhachHang.suaKhachHang(kh);
        }

        public bool xoaKhachHang(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Mã khách hàng không hợp lệ.");
            return _dalKhachHang.xoaKhachHang(id);
        }

        private void ValidateCustomer(CustomerReq kh)
        {
            if (string.IsNullOrWhiteSpace(kh.Name))
                throw new ArgumentException("Tên khách hàng không được để trống.");
            if (kh.Name.Length > 30)
                throw new ArgumentException("Tên khách hàng tối đa 30 ký tự.");
        }
    }
}
