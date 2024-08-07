using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace COMICSHOP.Areas.Admin.Data
{
    public class CTDHDisplayVM
    {
        public int IdChiTietDonHang { get; set; }
        public Nullable<int> IdDonHang { get; set; }
        public string TenKhach { get; set; }
        public string SoDienThoai { get; set; }
        public Nullable<double> TongTien { get; set; }
        [DisplayName("Ngày đặt")]
        public Nullable<System.DateTime> NgayDat { get; set; }
        [DisplayName("Địa chỉ")]
        public string DiaChi { get; set; }
        [DisplayName("Trạng thái")]
        public Nullable<byte> TrangThai { get; set; }
        public List<SanPhamVM> SanPhams { get; set; }
    }
}